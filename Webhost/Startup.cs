using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using System.Reflection;
using NetCore.WebHost.Models;
using NetCore.WebContext;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Autofac.Extras.DynamicProxy;
using NetCore.WebAPI.Middlewares;
using NetCore.Logging;

namespace NetCore.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient();

            services.AddHttpContextAccessor();

            services.AddMemoryCache();

            #region Cors policy

            services.AddCors();

            #endregion

            #region Json, Controller
            var builder = services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy'/'MM'/'dd HH':'mm':'ss.FFFFFFFK";
                });


            Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                .Where(x => x.Contains("WebAPI")).ToList()
                .Select(path => Assembly.LoadFrom(path))
                .ForEach(ass => builder.AddApplicationPart(ass));
            #endregion Json, Controller

            services.AddTransient<LoggingInterceptor>();
            // Add services to the container.
            #region using swagger
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(c =>
            {
                c.MapType<DateTime>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date",
                    Example = new DateTimeSampleProvider(DateTime.Now)
                });

                c.SwaggerDoc("Demo", new OpenApiInfo { Title = "Demo WebAPI", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.CustomSchemaIds(type => type.ToString());

                Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                    .ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
            });
            #endregion

            #region Cache

            services.AddOutputCaching(options =>
            {
                options.Profiles["Default"] = new WebEssentials.AspNetCore.OutputCaching.OutputCacheProfile
                {
                    Duration = this.Configuration.GetValue<double>("Cache:Default:Duration")
                };

                options.DoesRequestQualify = context =>
                {
                    if (context.Request.Method != HttpMethods.Get && context.Request.Method != HttpMethods.Post) return false;
                    if (context.User.Identity.IsAuthenticated) return false;

                    return true;
                };
            });

            #endregion Cache

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddMvc();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<WebAPI.Middlewares.SetDefautMiddleware>();
            app.UseMiddleware<NetCore.WebAPI.Middlewares.ReqRespMiddleware>();
            #region Cors Policy
            app.UseCors();
            #endregion Cors Policy

            #region Swagger
            var useSwagger = this.Configuration.GetValue<bool>("UseSwagger");
            if (useSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/Demo/swagger.json", "Demo WebAPI");
                        c.RoutePrefix = string.Empty;
                    }
                );

            }
            #endregion Swagger


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseOutputCaching();

            app.ApplicationServices.GetService<IHttpContextAccessor>().Configure();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        /// <summary>
        /// Autofac register
        /// </summary>
        /// <param name="builder">Autofac Container Builder</param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var prefixes = this.Configuration.GetValue<string>("AssemblyPrefixes")
                .Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            var assemblies = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
                .Select(path => Assembly.LoadFrom(path))
                .Select(x => x.GetName())
                .Where(x => prefixes.Any(p => x.Name.StartsWith(p)))
                .ToList();


            #region Assemblies

            var assArr = assemblies.Select(x => Assembly.Load(x)).ToArray();
            builder.RegisterAssemblyTypes(assArr).AsImplementedInterfaces().InstancePerLifetimeScope()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor));

            #endregion Assemblies

            #region Modules

            assemblies
                .SelectMany(x => Assembly.Load(x.Name).DefinedTypes)
                .Where(x => x.ImplementedInterfaces.Contains(typeof(NetCore.WebAPI.Interface.IAutofacModule)))
                .Distinct()
                .ForEach(type => builder.RegisterModule((Autofac.Module)Activator.CreateInstance(type, this.Configuration)));
            builder.RegisterModule(new Entities.AutofacModule(this.Configuration));

            #endregion Modules


            #region Logging

            assemblies.Where(x => x.Name.Contains("Plugins")).ToList()
                    .SelectMany(x => Assembly.Load(x.Name).DefinedTypes)
                    .Where(x => x.IsAssignableTo<NetCore.Logging.BaseLogProvider>())
                    .ForEach(type =>
                        builder.RegisterInstance(Activator.CreateInstance(type, this.Configuration)).AsSelf().AsImplementedInterfaces().SingleInstance());

            builder.RegisterType<Plugin.NLog.NLogProvider>().As<ILogProvider>();

            #endregion Logging


            builder.RegisterBuildCallback(c => ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(c)));
        }
    }
}


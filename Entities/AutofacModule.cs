using Autofac;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF.Trackable;
using Entities.Contexts;
using Autofac.Extras.DynamicProxy;
using NetCore.WebAPI.Middlewares;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class AutofacModule : Autofac.Module, NetCore.WebAPI.Interface.IAutofacModule
    {
        #region Initialization

        private readonly IConfiguration configuration;

        public AutofacModule(IConfiguration configuration) : base()
        {
            this.configuration = configuration;
        }

        #endregion Initialization

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<DemoDbContext>()
                .WithParameter("options", new DbContextOptionsBuilder<DemoDbContext>()

            //add nuget package Microsoft.EntityFrameworkCore.SqlServer
            //.UseSqlServer(this.configuration.GetConnectionString("Demo"), x => x.UseRelationalNulls()).Options)

            //add nuget package Oracle.EntityFrameworkCore
            //.UseOracle(this.configuration.GetConnectionString("Demo"),b => b.UseOracleSQLCompatibility("11")).Options)

            //add nuget package Pemelo.EntityFramewrokCore.MySql
            .UseMySql(this.configuration.GetConnectionString("Demo"), ServerVersion.AutoDetect(this.configuration.GetConnectionString("Demo")), x => x.UseRelationalNulls()).Options)
            .AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<DemoUnitOfWork>().As<IDemoUnitOfWork>().InstancePerLifetimeScope();

            #region Entities

            var entityTypes = Assembly.GetAssembly(typeof(DemoDbContext)).DefinedTypes
                  .Where(t => t.ImplementedInterfaces.Contains(typeof(TrackableEntities.Common.Core.ITrackable)));


            foreach (var type in entityTypes)
            {
                var repoType = typeof(TrackableRepository<>).MakeGenericType(type);
                var repoInterfaceType = typeof(ITrackableRepository<>).MakeGenericType(type);
                builder.Register(c => Activator.CreateInstance(repoType, c.Resolve<DemoDbContext>()))
                    .As(repoInterfaceType).InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor));
            }

            #endregion Entities
        }
    }
}


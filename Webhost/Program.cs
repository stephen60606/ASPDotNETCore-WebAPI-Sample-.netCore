using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using NetCore.WebHost;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                config.AddInMemoryCollection(AppSettingsProvider.GetConfigurations("appsettings.json", new List<string>() { "ConnectionStrings" }));
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
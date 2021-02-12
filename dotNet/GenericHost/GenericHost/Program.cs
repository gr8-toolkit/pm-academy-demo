using System;
using System.Threading.Tasks;
using GenericHost.ApplicationServices;
using GenericHost.HostedServices;
using GenericHost.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

namespace GenericHost
{
    class Program
    {
        static Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                    {
                        configurationBuilder.AddEnvironmentVariables(prefix: "GH_");
                        configurationBuilder.AddCommandLine(args);
                        configurationBuilder.AddJsonFile("settings.json", false, false);
                    })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddSerilog(new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File("app.log")
                        .CreateLogger());
                    //loggingBuilder.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.AddTransient(provider => new JsonWorker());
                    services.AddTransient<PrimesFinder>();
                    services.AddHostedService<FirstHostedService>();
                    services.AddHostedService<CPULoadHostedService>();
                    services.AddHostedService<LastHostedService>();
                })
                .ConfigureServices((context, services) =>
                {
                    //context.Configuration
                    var value = context.Configuration["primesFrom"];
                    services.AddOptions();
                    services.Configure<Settings>(context.Configuration);
                    services.PostConfigure<Settings>(value =>
                    {
                        ;
                    });
                });

            // configure host

            IHost host = builder.Build();


            return host.RunAsync();
        }
    }
}

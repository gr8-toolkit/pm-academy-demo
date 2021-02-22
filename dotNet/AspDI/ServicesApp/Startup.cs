using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicesApp.Services;

namespace ServicesApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITransientService, TransientService>();
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddScoped<IScopedService, ScopedService>();

            #pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            var ioc1 = services.BuildServiceProvider();
            var ioc2 = services.BuildServiceProvider();
            #pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            var singleton1 = ioc1.GetRequiredService<ISingletonService>();
            var singleton2 = ioc1.GetRequiredService<ISingletonService>();
            var singleton3 = ioc2.GetRequiredService<ISingletonService>();
            singleton1.Value = 42;
            
            Debug.Assert(singleton1.Value == singleton2.Value);
            Debug.Assert(singleton1.Value != singleton3.Value);

            var transient1 = ioc1.GetRequiredService<ITransientService>();
            var transient2 = ioc1.GetRequiredService<ITransientService>();

            transient1.Value = 42;
            Debug.Assert(transient1.Value != transient2.Value);

            var factory = ioc1.GetRequiredService<IServiceScopeFactory>();
            using var scope = factory.CreateScope();
            var scoped1 = scope.ServiceProvider.GetRequiredService<IScopedService>();
            var scoped2 = scope.ServiceProvider.GetRequiredService<IScopedService>();
            scoped1.Value = 42;
            Debug.Assert(scoped1.Value == scoped2.Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ISingletonService singleton)
        {
            // Setup singleton
            singleton.Value = 36;

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello Services!");
                });
            });
        }
    }
}

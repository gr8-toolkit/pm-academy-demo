using DependencyInjectionExampleApp.Extensions;
using DependencyInjectionExampleApp.Middlewares;
using DependencyInjectionExampleApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IOperationTransient, OperationExample>();
            services.AddScoped<IOperationScoped, OperationExample>();
            services.AddSingleton<IOperationSingleton, OperationExample>();
            //----------------------------------------------------------------------------------------------
            // controlled by IServiceCollection container
            services.AddTransient<DisposableServiceM1>();
            services.AddSingleton<IDisposableServiceM2>(v => new DisposableServiceM2(new LoggerFactory()));
            //----------------------------------------------------------------------------------------------
            // controller by user
            services.AddSingleton(new DisposableServiceM3(new LoggerFactory()));
            //----------------------------------------------------------------------------------------------
            services.AddTransient<FactoryActivatedMiddleware>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseConventionalMiddleware();
            app.UseFactoryActivatedMiddleware();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

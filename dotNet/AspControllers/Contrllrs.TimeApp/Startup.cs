using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Contrllrs.TimeApp.Services;
using Microsoft.AspNetCore.Http;

namespace Contrllrs.TimeApp
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
            // App services
            // Add TimeService as Singleton <Definition, Implementation>
            services.AddSingleton<ITimeService, TimeService>();

            // Framework services 
            services.AddControllers();

            // 3rd party services 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contrllrs.TimeApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Handle time request with middleware
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.StartsWith("/api/mw/time") && 
                    context.Request.Method == "GET")
                {
                    var timeService = new TimeService();
                    await context.Response.WriteAsync(timeService.GetServerTime());
                }
                else await next();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contrllrs.TimeApp v1"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Handle time request with endpoints
                endpoints.Map("/api/point/time", async context =>
                {
                    var timeService = new TimeService();
                    await context.Response.WriteAsync(timeService.GetServerTime());
                });

                // Handle requests with ASP.NET Core controllers
                endpoints.MapControllers();
            });
        }
    }
}

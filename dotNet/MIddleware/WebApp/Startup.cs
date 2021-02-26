using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Authorization;
using WebApp.Extensions;
using WebApp.Middlewares;
using WebApp.Services;

namespace WebApp
{

    public class Startup
    {
        private IWebHostEnvironment _env;
        public IConfiguration _config;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(opt => opt.LowercaseUrls = true);
            services.AddRazorPages();
            services.AddControllers();
            services.AddSwagger();
            services.AddCorsForAll("allow-all");
            services.AddRapidAuthentication((token) => VerifyToken(token));
            services.AddMediatR(typeof(Startup));
            services.AddMemoryCache();
            services.AddSingleton<IDataService, InMemoryDataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();
            app.UseCors("allow-all");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Application Name: {_env.ApplicationName}");
                });
            });

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.All,
            //});
        }

        private bool VerifyToken(string token)
        {
            return !string.IsNullOrEmpty(token);
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DepsWebApp.Clients;
using DepsWebApp.Options;
using DepsWebApp.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DepsWebApp
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
            // Add options
            services
                .Configure<CacheOptions>(Configuration.GetSection("Cache"))
                .Configure<NbuClientOptions>(Configuration.GetSection("Client"))
                .Configure<RatesOptions>(Configuration.GetSection("Rates"));
            
            // Add application services
            services.AddScoped<IRatesService, RatesService>();

            // Add NbuClient as Transient
            services.AddHttpClient<IRatesProviderClient, NbuClient>()
                .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(10));

            // Add CacheHostedService as Singleton
            services.AddHostedService<CacheHostedService>();

            // Add batch of Swashbuckle Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DI Demo App API", Version = "v1" });
            });

            // Add batch of framework services
            services.AddMemoryCache();
            services.AddControllers();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DI Demo App API v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

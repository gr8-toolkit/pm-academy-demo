using Contrllrs.Notes.Models;
using Contrllrs.Notes.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Contrllrs.Notes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IStorage<Note>, Storage<Note>>();
            //services.AddSingleton<IStorage<User>, Storage<User>>();
            
            // Alternative way of IStorage<Note>, IStorage<User> registration :
            services.AddSingleton(typeof(IStorage<>), typeof(Storage<>));

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // Disable auto BadRequest response
                options.SuppressModelStateInvalidFilter = true;
                // Disable auto error explanations 
                options.SuppressMapClientErrors = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contrllrs.Notes", Version = "v1" });
            });
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contrllrs.Notes v1"));
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

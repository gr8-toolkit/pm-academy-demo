using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Authorization;
using Microsoft.OpenApi.Models;
using System.IO;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRapidAuthentication(this IServiceCollection services, Predicate<string> func)
        {
            services.AddAuthentication(Consts.DefaultScheme)
                .AddScheme<RapidAuthenticationOptions, RapidServiceAuthenticationHandler>(Consts.RapidService, delegate (RapidAuthenticationOptions opts) { opts.VerifyToken = func; })
                .AddScheme<RapidAuthenticationOptions, RapidUserAuthenticationHandler>(Consts.RapidUser, delegate (RapidAuthenticationOptions opts) { opts.VerifyToken = func; });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var basePath = AppContext.BaseDirectory;
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(basePath, "WebApp.xml"));
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void AddCorsForAll(this IServiceCollection services, string policyName)
        {
            // https://www.infoworld.com/article/3327562/how-to-enable-cors-in-aspnet-core.html
            services.AddCors(o =>
            {
                o.AddPolicy(policyName,
                    builder =>
                    {
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });
        }
    }
}

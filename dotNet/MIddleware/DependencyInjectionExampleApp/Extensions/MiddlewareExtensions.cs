using DependencyInjectionExampleApp.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseConventionalMiddleware(
              this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConventionalMiddleware>();
        }

        public static IApplicationBuilder UseFactoryActivatedMiddleware(
            this IApplicationBuilder builder)
        {
            // It isn't possible to pass objects to the factory-activated middleware with UseMiddleware
            return builder.UseMiddleware<FactoryActivatedMiddleware>();
        }
    }
}

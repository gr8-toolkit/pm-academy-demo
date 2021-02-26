using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestableWebApi.Middlewares;

namespace TestableWebApi.Extensions
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

        public static IApplicationBuilder UsePipelineLoggingMiddleware(
            this IApplicationBuilder builder)
        {
            var exceptions = PrepareWebApiExceptions();
            return builder.UseMiddleware<PipelineLoggingMiddleware>(exceptions);
        }

        public static IApplicationBuilder UseStatusService(this IApplicationBuilder builder, string path = "/status", string name = "test-app", string team = "pm.tech.academy")
        {
            return builder.Map(path, v => v.UseMiddleware<ServiceStatusMiddleware>(name, team));
        }

        private static IEnumerable<string> PrepareWebApiExceptions()
        {
            yield return "/health";
            yield return "/status";
            yield return "/metrics";
            yield return "/help";
            yield return "/swagger";
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineExampleApp.Middlewares
{
    public class RoutingMiddleware
    {
        private readonly RequestDelegate _next;

        public RoutingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();

            if (path == "/index")
            {
                await context.Response.WriteAsync("common content");
            }
            else if (path == "/private")
            {
                await context.Response.WriteAsync("secured content");
            }

            context.Response.StatusCode = 404;
        }
    }
}

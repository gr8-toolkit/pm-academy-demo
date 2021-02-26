using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineExampleApp.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
            if (path == "/private" &&
                (!context.Request.Headers.TryGetValue("token", out StringValues tokenValue) ||
                string.IsNullOrEmpty(tokenValue[0])))
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            }

            return _next.Invoke(context);
        }
    }
}

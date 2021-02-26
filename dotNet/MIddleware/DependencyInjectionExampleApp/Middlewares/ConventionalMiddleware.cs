using DependencyInjectionExampleApp.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Middlewares
{
    public class ConventionalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOperationTransient _transient;
        private readonly IOperationSingleton _singleton;

        public ConventionalMiddleware(RequestDelegate next, IOperationTransient transient, IOperationSingleton singleton)
        {
            _next = next;
            _transient = transient;
            _singleton = singleton;
        }

        public async Task InvokeAsync(HttpContext context, IOperationTransient transient, IOperationScoped scoped, IOperationSingleton singleton)
        {
            context.Response.Headers.Add("X-Conventional", new Microsoft.Extensions.Primitives.StringValues("true"));
            context.Response.Headers.Add("X-Conventional-Transient-M1", new Microsoft.Extensions.Primitives.StringValues(_transient.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Singleton-M1", new Microsoft.Extensions.Primitives.StringValues(_singleton?.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Scoped-M1", new Microsoft.Extensions.Primitives.StringValues(scoped?.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Transient-M2", new Microsoft.Extensions.Primitives.StringValues(_transient.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Singleton-M2", new Microsoft.Extensions.Primitives.StringValues(_singleton?.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Scoped-M2", new Microsoft.Extensions.Primitives.StringValues(scoped?.State.ToString()));


            context.Response.Headers.Add("X-Conventional-Transient-M3", new Microsoft.Extensions.Primitives.StringValues(transient.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Singleton-M3", new Microsoft.Extensions.Primitives.StringValues(singleton?.State.ToString()));

            context.Response.Headers.Add("X-Conventional-Transient-M4", new Microsoft.Extensions.Primitives.StringValues(transient.State.ToString()));
            context.Response.Headers.Add("X-Conventional-Singleton-M4", new Microsoft.Extensions.Primitives.StringValues(singleton?.State.ToString()));

            await _next(context);
        }
    }
}

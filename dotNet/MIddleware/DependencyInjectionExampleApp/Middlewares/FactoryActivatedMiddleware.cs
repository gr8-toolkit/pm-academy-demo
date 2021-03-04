using DependencyInjectionExampleApp.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Middlewares
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private readonly IOperationTransient _transient;
        private readonly IOperationSingleton _singleton;

        public FactoryActivatedMiddleware(IOperationTransient transient, IOperationSingleton singleton)
        {
            _transient = transient;
            _singleton = singleton;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("X-Factory", new Microsoft.Extensions.Primitives.StringValues("true"));
            context.Response.Headers.Add("X-Factory-Transient-M1", new Microsoft.Extensions.Primitives.StringValues(_transient.State.ToString()));
            context.Response.Headers.Add("X-Factory-Singleton-M1", new Microsoft.Extensions.Primitives.StringValues(_singleton.State.ToString()));
            context.Response.Headers.Add("X-Factory-Transient-M2", new Microsoft.Extensions.Primitives.StringValues(_transient.State.ToString()));
            context.Response.Headers.Add("X-Factory-Singleton-M2", new Microsoft.Extensions.Primitives.StringValues(_singleton.State.ToString()));
            await next(context);
        }
    }
}

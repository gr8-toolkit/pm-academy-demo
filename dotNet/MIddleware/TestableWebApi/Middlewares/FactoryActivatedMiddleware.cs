using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestableWebApi.Middlewares
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("X-Factory", new Microsoft.Extensions.Primitives.StringValues("true"));
            await next(context);
        }
    }
}

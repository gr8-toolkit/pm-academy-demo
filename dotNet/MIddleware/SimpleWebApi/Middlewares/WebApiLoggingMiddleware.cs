using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebApi.Middlewares
{
    public class WebApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<string> _exceptions;

        public WebApiLoggingMiddleware(RequestDelegate next, IEnumerable<string> exceptoins)
        {
            _next = next;
            _exceptions = exceptoins;
        }

        public Task Invoke(HttpContext context)
        {
            if (!SkipThisMiddleware(context))
            {
                InvokeInternal(context);
            }
            return InvokeNext(context);
        }

        private Task InvokeInternal(HttpContext context)
        {
            return Task.CompletedTask;
        }

        private Task InvokeNext(HttpContext context)
        {
            return _next(context);
        }

        private bool SkipThisMiddleware(HttpContext context)
        {
            foreach (var exc in _exceptions)
            {
                if (context.Request.Path.Value.StartsWith(exc))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

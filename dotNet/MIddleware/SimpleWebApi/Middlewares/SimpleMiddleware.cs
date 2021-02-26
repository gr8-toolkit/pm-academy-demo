using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Middlewares
{
    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;

        public SimpleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            return _next(context);
        }

        // 'Multiple public 'Invoke' or 'InvokeAsync' methods are available.'
        //public async Task InvokeAsync(HttpContext context)
        //{
        //    await _next(context);
        //}
    }
}

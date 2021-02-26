using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PipelineExampleApp.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                Task processStatus;
                switch (context.Response.StatusCode)
                {
                    case 403:
                        processStatus = context.Response.WriteAsync("Access denied.");
                        break;
                    case 404:
                        processStatus = context.Response.WriteAsync("Not found.");
                        break;
                    default:
                        processStatus = Task.CompletedTask;
                        break;
                }

                await processStatus;
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}

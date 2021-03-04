using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Middlewares
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
            }
            catch (AggregateException exp)
            {
                await HandleExceptionAsync(context, exp.GetBaseException(), HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception e, HttpStatusCode code)
        {
            var result = JsonConvert.SerializeObject(new
            {
                Code = code,
                e.Message,
                e.StackTrace,
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}

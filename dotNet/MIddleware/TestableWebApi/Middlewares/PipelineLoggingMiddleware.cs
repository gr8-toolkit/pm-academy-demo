using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestableWebApi.Extensions;

namespace TestableWebApi.Middlewares
{
    public class PipelineLoggingMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        private readonly ILogger<PipelineLoggingMiddleware> _logger;
        private readonly IEnumerable<string> _exceptions;


        public PipelineLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IConfiguration configuration, IEnumerable<string> exceptoins)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = loggerFactory != null ? loggerFactory.CreateLogger<PipelineLoggingMiddleware>() : throw new ArgumentNullException(nameof(loggerFactory));
            _next = next;
            _exceptions = exceptoins;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!SkipThisMiddleware(context))
            {
                await InvokeInternalAsync(context);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task InvokeInternalAsync(HttpContext context)
        {
            var correlationId = Guid.NewGuid().ToString("N");

            using (MemoryStream requestBodyStream = new MemoryStream())
            {
                using (MemoryStream responseBodyStream = new MemoryStream())
                {
                    Stream originalRequestBody = context.Request.Body;
                    //context.Request.EnableRewind();
                    Stream originalResponseBody = context.Response.Body;

                    try
                    {
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        var requestBody = new StreamReader(requestBodyStream).ReadToEnd().Replace("\r", string.Empty).Replace("\n", string.Empty);

                        requestBodyStream.Seek(0, SeekOrigin.Begin);
                        context.Request.Body = requestBodyStream;

                        var responseBody = string.Empty;
                        context.Response.Body = responseBodyStream;
                        context.Response.Headers.Add("X-Correlation-Id", new Microsoft.Extensions.Primitives.StringValues(correlationId));

                        _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path.Value} starting with correlationId: {correlationId}, requestHeaders: [{context.Request.Headers.PlainText()}], requestBody: [{requestBody}]");
                        var sw = Stopwatch.StartNew();

                        using (LogContext.PushProperty("Correlation-Id", correlationId))
                        {
                            await _next(context);
                        }

                        sw.Stop();

                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                        responseBodyStream.Seek(0, SeekOrigin.Begin);

                        await responseBodyStream.CopyToAsync(originalResponseBody);
                        _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path.Value} finished in {sw.ElapsedMilliseconds}ms [{context.Response.StatusCode}] with correlationId: {correlationId}, responseHeaders: [{context.Response.Headers.PlainText()}], responseBody: [{responseBody}] ");
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Unable to handle request with correlationId: {correlationId}", e);
                    }
                    finally
                    {
                        context.Request.Body = originalRequestBody;
                        context.Response.Body = originalResponseBody;
                    }
                }
            }
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

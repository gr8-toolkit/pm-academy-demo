using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestableWebApi.Common;

namespace TestableWebApi.Middlewares
{
    public class ServiceStatusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _info;

        public ServiceStatusMiddleware(RequestDelegate next, string name, string team)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            var map = new Dictionary<string, string>
            {
                ["name"] = name,
                ["team"] = team,
            };

            var sb = new StringBuilder();
            sb.AppendFormat("service_status {{{0}}}", string.Join(",", map.Select(pair => $"{pair.Key}=\"{pair.Value}\"")));
            
            _info = sb.ToString();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "text/plain";
            httpContext.Response.StatusCode = 200;

            var info = new AppInfo();

            var sb = new StringBuilder();
            sb.Append(_info);
            sb.AppendLine();
            sb.AppendFormat("service_uptime {0}", info.UptimeSeconds);
            sb.AppendLine();
            sb.AppendLine();

            await httpContext.Response.WriteAsync(sb.ToString());
        }
    }
}

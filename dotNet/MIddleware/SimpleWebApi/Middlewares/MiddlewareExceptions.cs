using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Middlewares
{
    public static class MiddlewareExceptions
    {
        public static IEnumerable<string> WebApi { get; } = PrepareWebApiExceptoins();

        private static IEnumerable<string> PrepareWebApiExceptoins()
        {
            yield return "/health";
            yield return "/status";
            yield return "/metrics";
            yield return "/help";
            yield return "/swagger";
        }
    }
}

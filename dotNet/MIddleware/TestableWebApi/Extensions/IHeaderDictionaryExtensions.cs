using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestableWebApi.Extensions
{
    public static class IHeaderDictionaryExtensions
    {
        public static string PlainText(this Microsoft.AspNetCore.Http.IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                return string.Empty;
            }

            var headers = headerDictionary
                .Keys
                .Select(key => $"{key}: {headerDictionary[key]}");

            return string.Join(", ", headers);
        }
    }
}

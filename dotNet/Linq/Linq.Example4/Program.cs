using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Example4
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new[] {1, 20, 300, 4000};
            var lengths = numbers.Where(n => n > 10).ToStringLengths();
            foreach (var length in lengths)
            {
                Console.WriteLine(length);
            }
        }
    }

    public static class MyExtensions
    {
        public static IEnumerable<string> ToStringLengths<T>(this IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                //Console.WriteLine($"Processing : {item}");
                var str = item?.ToString();
                yield return $"{str}[{str?.Length}]";
            }
        }

        public static IEnumerable<string> ToStringLengths2<T>(this IEnumerable<T> source)
        {
            return source.Select(item => item?.ToString()).Select(str => $"{str}[{str?.Length}]");
        }
    }
}

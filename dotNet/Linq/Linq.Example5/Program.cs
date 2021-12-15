using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Example5
{
    internal static class Program
    {
        private static void Main()
        {
            var numbers = new[] { 1, 20, 300, 4000 };
            
            // Multiple enumeration problem
            //var lengths = numbers.Where(n => n > 10).ToStringLengths();

            // Correct enumeration with possible lazy exception
            var lengths = numbers.Where(n => n > 10).ToStringLengths().ToArray();

            var pow2 = lengths.Select(len => len + " "+ len);
            var pow3 = lengths.Select(len => len + " " + len + " " + len);
            
            // Place for Lazy exceptions
            foreach (var length in pow2) Console.WriteLine(length);
            foreach (var length in pow3) Console.WriteLine(length);
        }
    }

    public static class MyExtensions
    {
        public static IEnumerable<string> ToStringLengths<T>(this IEnumerable<T> source)
        {
            // Lazy exception
            //throw new NotImplementedException("Lazy NotImplementedException");

            foreach (var item in source)
            {
                Console.WriteLine($"Processing : {item}");
                var str = item?.ToString();
                yield return $"{str}[{str?.Length}]";
            }
        }
    }
}

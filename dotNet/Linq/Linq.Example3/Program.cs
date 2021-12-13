using Linq.Data;
using System;
using System.Linq;

namespace Linq.Example3
{
    internal static class Program
    {
        private static readonly Person[] People =
        {
            new("Max", 34),
            new("John", 19),
            new("Andrew", 62),
        };

        private static void Main()
        {
            var c = new { SomeProp = "prop" };
            Console.WriteLine(c.GetType().Name);

            var people = People.Select(p => new
            {
                p.Name,
                Honorable = p.Age > 60 ? "Sir" : "Dear"
            });

            foreach (var p in people)
            {
                Console.WriteLine(p);
            }
        }
    }
}

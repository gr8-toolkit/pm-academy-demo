using Linq.Data;
using System;
using System.Linq;

namespace Linq.Example3
{
    class Program
    {
        private static readonly Person[] People =
        {
            new Person("Max", 34),
            new Person("John", 19),
            new Person("Andrew", 62),
        };

        static void Main(string[] args)
        {
            var c = new { SomeProp = "prop" };
            Console.WriteLine(c.GetType().Name);

            var people = People.Select(p => new
            {
                Name = p.Name,
                Honorable = p.Age > 60 ? "Sir" : "Dear"
            });

            foreach (var p in people)
            {
                Console.WriteLine(p);
            }
        }
    }
}

using System;

namespace Loops.Foreach.Example1
{
    class Program
    {
        static void Main()
        {
            Print();
        }

        private static void Print()
        {
            var seasons = new[] { "Winter", "Spring", "Summer", "Autumn" };
            foreach (var season in seasons)
            {
                Console.WriteLine(season);
            }
            Console.WriteLine("Done");
        }
    }
}

using System;

namespace Loops.For.Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            Print();
        }

        private static void Print()
        {
            var seasons = new[] { "Winter", "Spring", "Summer", "Autumn" };
            for (int n = 0; n < seasons.Length; n++)
            {
                Console.WriteLine(seasons[n]);
            }
            Console.WriteLine("Done");
        }
    }
}
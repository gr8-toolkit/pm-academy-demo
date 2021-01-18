using System;

namespace Loops.While.Example2
{
    class Program
    {
        static void Main()
        {
            Print();
        }

        private static void Print()
        {
            var seasons = new[] {"Winter", "Spring", "Summer", "Autumn"};
            var n = 0;
            while (n < seasons.Length)
            {
                Console.WriteLine(seasons[n]);
                n++;
            }
        }
    }
}

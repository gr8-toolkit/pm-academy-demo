using System;
using System.Collections.Generic;

namespace Loops.Foreach.Example2
{
    class Program
    {
        static void Main()
        {
            var seasons = new[] { "Winter", "Spring", "Summer", "Autumn" };
            PrintForeach(seasons);
            PrintWhile(seasons);
        }

        private static void PrintForeach(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Foreach done");
        }

        private static void PrintWhile(IEnumerable<string> items)
        {
            // very simplified foreach implementation via while loop
            using var enumerator = items.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                string item = enumerator.Current;
                Console.WriteLine(item);
            }
            Console.WriteLine("While done");
        }
    }
}

using System;

namespace Loops.For.Example3
{
    class Program
    {
        static void Main()
        {
            var array = new[] { "Johnny", "Max", "Jane", "Christopher", "Alex", "Roy" };
            PrintLongest(array);
        }

        static void PrintLongest(string[] array)
        {
            if (array.Length < 1)
            {
                Console.WriteLine("Empty array");
            }

            var longest = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Length > longest.Length) longest = array[i];
            }
            Console.WriteLine($"The longest string is : {longest}");
        }
    }
}

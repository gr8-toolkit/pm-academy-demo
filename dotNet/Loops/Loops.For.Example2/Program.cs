using System;

namespace Loops.For.Example2
{
    class Program
    {
        static void Main()
        {
            var array = new[] {1, 2, 6, 9, 3, 8};
            PrintMax(array);
        }

        static void PrintMax(int[] array)
        {
            if (array.Length < 1)
            {
                Console.WriteLine("Empty array");
            }

            var max = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > max) max = array[i];
            }
            Console.WriteLine($"The max value is : {max}");
        }
    }
}

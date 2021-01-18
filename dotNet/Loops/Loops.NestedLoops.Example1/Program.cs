using System;

namespace Loops.NestedLoops.Example1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Multiplication table :");
            PrintMultiTable(1, 9);
        }

        private static void PrintMultiTable(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                for (int j = min; j <= max; j++)
                {
                    Console.WriteLine($"{i} x {j} = {i*j}");
                }
            }
        }
    }
}

using System;

namespace Loops.NestedLoops.Example2
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintPrime(1, 100);
        }

        private static void PrintPrime(int low, int high)
        {
            for (var i = Math.Max(low, 2); i <= high; i++)
            {
                var isPrime = true;
                for (var j = 2; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (!isPrime) continue;

                Console.WriteLine($"{i} is prime");
            }
        }
    }
}

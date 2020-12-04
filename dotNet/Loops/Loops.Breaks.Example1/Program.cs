using System;

namespace Loops.Breaks.Example1
{
    class Program
    {
        static void Main()
        {
            StringLengthCounter();
        }

        private static void StringLengthCounter()
        {
            while (true)
            {
                Console.Write("Input : ");
                var input = Console.ReadLine();

                // skip empty strings
                if (string.IsNullOrWhiteSpace(input)) continue;

                // check stop-word 'exit'
                if (input == "exit") break;

                // print result
                Console.WriteLine($"Length of '{input}' is {input.Length}");
            }
        }
    }
}

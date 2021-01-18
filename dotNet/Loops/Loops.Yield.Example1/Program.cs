using System;
using System.Collections;

namespace Loops.Yield.Example1
{
    class Program
    {
        static void Main()
        {
            PrintHandbag();
        }

        static IEnumerable Handbag()
        {
            yield return 42;
            yield return new Exception("Oops");
            yield return null;
            yield return "gun";
            yield break;
            yield return "Unreachable code";
        }

        private static void PrintHandbag()
        {
            Console.WriteLine("Handbag :");
            foreach (var item in Handbag())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("That's all");
        }
    }
}

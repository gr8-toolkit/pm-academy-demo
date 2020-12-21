using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Example1
{
    class Program
    {
        static void Main()
        {
            var numbers = new[] {1, -2, 7, 9, -3};
            var positive = 
                from number in numbers 
                let square = number * number
                where number >= 0 
                select square;
            
            Print(positive);

            positive = numbers.Where(number => number >= 0);
            Print(positive);
        }

        private static void Print<T>(IEnumerable<T> collection)
        {
            Console.WriteLine("Result :");
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}

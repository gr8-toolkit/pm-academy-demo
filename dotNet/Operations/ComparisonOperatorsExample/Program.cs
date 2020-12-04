using System;

namespace ComparisonOperatorsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ComparisonOperatorsExample");
            int n1 = 5, n2 = 6, n3 = 5;
            bool a1 = n1 == n2;                     // false
            bool a2 = n2 != n3;                     // true
            bool a4 = n1 < n2;                      // true
            bool a5 = n2 >= n1;                     // true

            bool b1 = a1 && a2;                     // false
            bool b2 = n1 == n2 && n2 != n3;         // false
            bool b3 = a1 || a2;                     // true
            bool b4 = !a1;                          // true
        }
    }
}

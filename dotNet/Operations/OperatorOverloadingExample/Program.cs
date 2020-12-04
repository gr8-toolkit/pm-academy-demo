using OperatorOverloadingExample.Custom;
using System;

namespace OperatorOverloadingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OperatorOverloadingExample");

            var f150 = new F150(300);
            var mustang = new Mustang(400);
            mustang <<= 12;                         // 400 << 12 = 1638400
            var power2 = 400 << 12;                 // 1638400
            var equ = mustang.Power == power2;      // true
        }
    }
}

using System;

namespace ThreadBlockingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadSleepExample.ExecuteM1();
            //ThreadJoinExample.ExecuteM1();
            Console.ReadKey();
        }
    }
}

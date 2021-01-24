using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadBlockingExample
{
    public static class ThreadSleepExample
    {
        private static int _counter;

        public static void ExecuteM1()
        {
            Console.WriteLine("Starting M1 ...");

            var foo = new Thread(Foo);
            var bar = new Thread(Bar);
            foo.Start();
            bar.Start();

            Console.WriteLine("Finished M1 ...");
        }


        static void Foo()
        {
            Console.WriteLine("Foo enter");
            for (_counter = 0; _counter < 100; _counter++)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine($"Counter in Foo: {_counter}");
            Console.WriteLine("Foo exit");
        }

        static void Bar()
        {
            Console.WriteLine("Bar enter");
            Thread.Sleep(2000);
            Console.WriteLine($"Counter in Bar: {_counter}");
            Console.WriteLine("Bar exit");
        }
    }
}

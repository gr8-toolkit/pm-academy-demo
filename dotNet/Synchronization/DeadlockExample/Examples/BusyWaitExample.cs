using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationProblems
{
    public static class BusyWaitExample
    {
        private static int _counter;

        public static void ExecuteM1()
        {
            Console.WriteLine("Starting M1 ...");

            var foo = Task.Run(Foo);
            var bar = Task.Run(Bar);
            Task.WaitAll(foo, bar);

            Console.WriteLine("Finished M1 ...");
        }

        static void Foo()
        {
            Console.WriteLine("Foo enter");
            for (_counter = 10; _counter >= 0; _counter--)
            {
                Thread.Sleep(100);
                Console.WriteLine("Decrement counter in Foo");
            }
            Console.WriteLine("Foo exit");
        }

        static void Bar()
        {
            Console.WriteLine("Bar enter");
            while (_counter != 1)
            {
                Console.WriteLine("waiting in Bar ...");
            }
            Console.WriteLine("Bar exit");
        }
    }
}

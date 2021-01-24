using System;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationProblems
{
    public static class NestedLockExample
    {
        static readonly object _lock1 = new object();
        static readonly object _lock2 = new object();

        public static void ExecuteM1()
        {
            Console.WriteLine("Starting M1 ...");

            var foo = new Thread(Foo);
            var bar = new Thread(Bar);
            foo.Start();
            bar.Start();

            Console.WriteLine("Finished M1 ...");
        }

        public static void ExecuteM2()
        {
            Console.WriteLine("Starting M2 ...");

            var foo = Task.Run(Foo);
            var bar = Task.Run(Bar);
            Task.WaitAll(foo, bar);

            Console.WriteLine("Finished M2 ...");
        }

        public static void ExecuteM3()
        {
            Console.WriteLine("Starting M3 ...");

            var foo = Task.Run(Foo);
            foo.Wait();

            var bar = Task.Run(Bar);
            bar.Wait();

            Console.WriteLine("Finished M3 ...");
        }


        static void Foo()
        {
            Console.WriteLine("Foo enter");
            lock (_lock1)
            {
                Console.WriteLine("working in Foo #1");
                Thread.Sleep(100);
                Console.WriteLine("waits for Bar lock");
                lock (_lock2)
                {
                    Console.WriteLine("working in Foo #2");
                }
            }
            Console.WriteLine("Foo exit");
        }

        static void Bar()
        {
            Console.WriteLine("Bar enter");
            lock (_lock2)
            {
                Console.WriteLine("working in Bar #1");
                Thread.Sleep(100);
                Console.WriteLine("waits for Foo lock");
                lock (_lock1)
                {
                    Console.WriteLine("working in Bar #2");
                }
            }
            Console.WriteLine("Bar exit");
        }
    }
}
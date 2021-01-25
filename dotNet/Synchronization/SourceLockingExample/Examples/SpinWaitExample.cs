using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Common;

namespace SourceLockingExample
{
    public static class SpinWaitExample
    {
        private static int _shared;

        public static void Execute()
        {
            var foo = new Thread(Foo);
            var bar = new Thread(Bar);
            foo.Start();
            bar.Start();

            //var stack = new LockFreeStack<int>();
        }

        static void Foo()
        {
            Console.WriteLine("Foo enter");
            var spin = new SpinWait();

            while (true)
            {
                if (Interlocked.CompareExchange(ref _shared, 5, 10) == 5)
                {
                    Console.WriteLine("Executing Foo");
                    Thread.Sleep(1000);
                    break;
                }
                spin.SpinOnce();
            }
            Console.WriteLine("Foo exit");
        }

        static void Bar()
        {
            Console.WriteLine("Bar enter");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Executing Bar {i + 1}");
                Thread.Sleep(100);
                _shared++;
            }
            Console.WriteLine("Bar exit");
        }
    }
}

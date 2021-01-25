using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceLockingExample
{
    public static class SemaphoreSlimExample3
    {
        static SemaphoreSlim _sem = new SemaphoreSlim(2);
        static int _count = 10;

        public static void Execute()
        {
            Foo();
        }

        static void Foo()
        {
            for (int i = 0; i <= _count; i++)
            {
                var thread = new Thread(Bar)
                {
                    Name = i.ToString()
                };
                thread.Start();
            }
        }

        static void Bar()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} wants to enter ");
            _sem.Wait();
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} has entered section");
            Thread.Sleep(10000);
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} has leaved section");
            _sem.Release();
        }
    }
}

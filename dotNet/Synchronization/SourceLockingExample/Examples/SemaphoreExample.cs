using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceLockingExample
{
    // Ensures not more than a specified number of concurrent threads can access a resource, or section of code
    public static class SemaphoreExample
    {
        static Semaphore _semafore;
        static int _counter = 3;

        public static void Execute()
        {
            Foo(false);
        }

        static void Foo(bool exclusive)
        {
            _semafore = exclusive ? new Semaphore(1, 1) : new Semaphore(3, _counter);

            for (int i = 0; i < 20; i++)
            {
                var thread = new Thread(Bar)
                {
                    Name = $"Bar #{i + 1}"
                };
                thread.Start();
            }
        }

        static void Bar()
        {
            while (_counter > 0)
            {
                _semafore.WaitOne();

                Console.WriteLine($"{Thread.CurrentThread.Name} enter");

                Console.WriteLine($"{Thread.CurrentThread.Name} working");
                Thread.Sleep(3000);

                Console.WriteLine($"{Thread.CurrentThread.Name} leave");

                _semafore.Release();

                _counter--;
                Thread.Sleep(1000);
            }
        }
    }
}

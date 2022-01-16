using System;
using System.Diagnostics;
using System.Threading;

namespace Threads.ManagedThread
{
    /// <summary>
    /// Demo for managed <see cref="Thread"/>.
    /// Prints thread deatils with and without sync.
    /// </summary>
    internal class Program
    {
        private static readonly object Sync = new();

        static void Main()
        {
            Console.WriteLine("Thread details demo");

            Console.WriteLine("---");
            Console.WriteLine("Thread details without sync :");
            MultiThreadDetailsTest(PrintThreadDetails);

            Console.WriteLine("---");
            Console.WriteLine("Thread details with sync :");
            MultiThreadDetailsTest(PrintThreadDetailsLock);
        }

        private static void PrintThreadDetailsLock()
        {
            // DEMO: Unhandled exception in thread
            //throw new InvalidOperationException("Oops!");

            lock (Sync)
            {
                PrintThreadDetails();
            }
        }

        private static void PrintThreadDetails()
        {
            var t = Thread.CurrentThread;
            var id = t.ManagedThreadId;
            Console.WriteLine($"Thread [{id}] name is {t.Name}");
            Console.WriteLine($"Thread [{id}] is BG {t.IsBackground}");
            Console.WriteLine($"Thread [{id}] priority {t.Priority}");
            Console.WriteLine($"Thread [{id}] state {t.ThreadState}");
            Console.WriteLine($"Thread [{id}] started not dead {t.IsAlive}");
            Console.WriteLine($"Thread [{id}] from pool {t.IsThreadPoolThread}");
        }

        private static void MultiThreadDetailsTest(ThreadStart entryPoint)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var t1 = new Thread(entryPoint)
            {
                Name = "T1", 
                IsBackground = false
            };

            var t2 = new Thread(entryPoint)
            {
                Name = "T2", 
                IsBackground = true
            };

            var t3 = new Thread(entryPoint)
            {
                Name = "T3",
                IsBackground = true
            };
            
            // Start all threads
            t1.Start();
            t2.Start();
            t3.Start();

            // Waiting for t1, t2, t3 threads completion 
            t1.Join();
            t2.Join();
            t3.Join();

            // DEMO : Suspend current thread
            Thread.Sleep(1000);
        }
    }
}

using System;
using System.Diagnostics;
using System.Threading;
using Threads.Shared;

namespace Threads.Pool
{
    /// <summary>
    /// Thread pool <see cref="ThreadPool"/> demo.
    /// </summary>
    internal class Program
    {
        private static readonly object Marker = new object();
        private static readonly CountdownEvent Waiter = new CountdownEvent(3);

        static void Main()
        {
            Console.WriteLine("ThreadPool demo");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // DEMO: add task to ThreadPool queue 
            ThreadPool.QueueUserWorkItem(FindPrimes, 1..100_000);
            ThreadPool.QueueUserWorkItem(FindPrimes, 100_000..150_000);
            ThreadPool.QueueUserWorkItem(FindPrimes, 150_000..200_000);

            // DEMO: Check ThreadPool working threads cnt changes
            Thread.Sleep(500);
            ThreadPool.GetMaxThreads(out var worker, out var io);
            Console.WriteLine($"MaxThreads : {worker}, {io}");

            ThreadPool.GetMinThreads(out worker, out io);
            Console.WriteLine($"MinThreads : {worker}, {io}");

            ThreadPool.GetAvailableThreads(out worker, out io);
            Console.WriteLine($"AvailableThreads : {worker}, {io}");

            // DEMO: waiting for thread-sync counter (count from 3 to 0)
            Waiter.Wait();

            stopwatch.Stop();

            // DEMO: Check ThreadPool working threads cnt changes
            Thread.Sleep(500);
            ThreadPool.GetAvailableThreads(out worker, out io);
            Console.WriteLine($"AvailableThreads : {worker}, {io}");

            Console.WriteLine("Elapsed total : {0}", stopwatch.Elapsed);
        }

        private static void PrintThreadDetailsLock()
        {
            lock (Marker)
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

        private static void FindPrimes(object obj)
        {
            PrintThreadDetailsLock();

            var range = (Range)obj;
            var comp = new Primes();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var primes = comp.FindPrimes(range.Start.Value, range.End.Value);

            stopwatch.Stop();
            Console.WriteLine("Primes : {0}", primes.Count);
            Console.WriteLine("Elapsed : {0}", stopwatch.Elapsed);
            
            // DEMO: decrement thread-sync counter
            Waiter.Signal();
        }
    }
}


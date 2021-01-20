using System;
using System.Diagnostics;
using System.Threading;
using Threads.Shared;

namespace Threads.ManagedThread
{
    class Program
    {
        private static readonly object Marker = new object();

        static void Main()
        {
            Console.WriteLine("Threads World!");
            
            PrintThreadDetails();
            SingleThreadTest();
            MultiThreadTest();
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

        private static void PrintThreadDetailsLock()
        {
            lock (Marker)
            {
                PrintThreadDetails();
            }
        }
        
        private static void SingleThreadTest()
        {
            //PrintThreadDetails();
            var comp = new Primes();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var primes1 = comp.FindPrimes(1, 100_000);
            var primes2 = comp.FindPrimes(100_000, 150_000);
            var primes3 = comp.FindPrimes(150_000, 200_000); // 8.3 sec

            stopwatch.Stop();

            Console.WriteLine("Primes : {0}", primes1.Count + primes2.Count + primes3.Count);
            Console.WriteLine("Elapsed : {0}", stopwatch.Elapsed);
        }

        private static void MultiThreadTest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var t1 = new Thread(FindPrimes)
            {
                Name = "T1", 
                IsBackground = false
            };
            t1.Start(1..100_000);

            var t2 = new Thread(FindPrimes)
            {
                Name = "T2", 
                IsBackground = true
            };
            t2.Start(100_000..150_000);

            var t3 = new Thread(FindPrimes)
            {
                Name = "T3",
                IsBackground = true
            };
            t3.Start(150_000..200_000);

            // Waiting for t1, t2, t3 threads completion 
            t1.Join();
            t2.Join();
            t3.Join();

            stopwatch.Stop();

            // DEMO : Suspend current thread
            Thread.Sleep(1000);
            
            Console.WriteLine("Elapsed total : {0}", stopwatch.Elapsed);
        }

        private static void FindPrimes(object obj)
        {
            // DEMO : concurrent access to shared resource (Console)
            //PrintThreadDetails();
            PrintThreadDetailsLock();

            var range = (Range) obj;
            var comp = new Primes();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var primes = comp.FindPrimes(range.Start.Value, range.End.Value);
            
            // DEMO: Unhandled exception in thread
            //throw new InvalidOperationException("Oops!");
            
            stopwatch.Stop();
            Console.WriteLine("Primes : {0}", primes.Count);
            Console.WriteLine("Elapsed : {0}", stopwatch.Elapsed);
        }
    }
}

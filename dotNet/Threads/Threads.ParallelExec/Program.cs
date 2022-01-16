using System;
using System.Diagnostics;
using System.Threading;
using Threads.Shared;

namespace Threads.ParallelExec
{
    /// <summary>
    /// Multithreading performance demo.
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Multithreading performance demo");

            Console.WriteLine("---");
            Console.WriteLine("Waiting for single thread test :");
            SingleThreadTest();

            Console.WriteLine("---");
            Console.WriteLine("Waiting for multi thread test :");
            MultiThreadTest();
        }

        private static void SingleThreadTest()
        {
            var comp = new Primes();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var primes1 = comp.FindPrimes(1, 100_000);
            var primes2 = comp.FindPrimes(100_000, 150_000);
            var primes3 = comp.FindPrimes(150_000, 200_000); // 2.1 sec

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

            Console.WriteLine("Elapsed total : {0}", stopwatch.Elapsed);
        }

        private static void FindPrimes(object obj)
        {
            var range = (Range)obj;
            var comp = new Primes();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var primes = comp.FindPrimes(range.Start.Value, range.End.Value);

            stopwatch.Stop();
            Console.WriteLine("Primes : {0} at {1}", primes.Count, stopwatch.Elapsed);
        }
    }
}
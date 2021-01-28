using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example10
{
    class Program
    {
        private static readonly object Locker = new object();
        static async Task Main()
        {
            Console.WriteLine("Hello Async!");
            var sw = new Stopwatch();
            sw.Restart();

            lock (Locker)
            {
                // Can't use await with lock!
                //var primes = await Primes.FromRangeAsync(1, 100_000, CancellationToken.None);
                sw.Stop();
                var elapsed = sw.Elapsed;
                //Console.WriteLine($"Found {primes.Count} primes after {elapsed}");
            }
        }
    }
}

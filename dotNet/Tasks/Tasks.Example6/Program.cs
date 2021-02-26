using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example6
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hello Async!");
            var sw = new Stopwatch();
            sw.Restart();
            var ct = CancellationToken.None;

            List<int> primes1 = await Primes.FromRangeAsync(1, 100_000, ct);
            List<int> primes2 = await Primes.FromRangeAsync(100_000, 150_000, ct);
            List<int> primes3 = await Primes.FromRangeAsync(150_000, 200_000, ct);

            int primes = primes1.Count + primes2.Count + primes3.Count;

            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Found {primes} primes after {elapsed}");
        }
    }
}

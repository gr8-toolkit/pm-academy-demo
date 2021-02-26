using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example7
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hello Async!");
            var sw = new Stopwatch();
            sw.Restart();
            var ct = CancellationToken.None;

            Task<List<int>> task1 = Primes.FromRangeAsync(1, 100_000, ct);
            Task<List<int>> task2 = Primes.FromRangeAsync(100_000, 150_000, ct);
            Task<List<int>> task3 = Primes.FromRangeAsync(150_000, 200_000, ct);
            
            List<int>[] results = await Task.WhenAll(task1, task2, task3);
            int primes = results.Sum(r => r.Count);

            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Found {primes} primes after {elapsed}");
        }
    }
}

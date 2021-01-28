using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example5
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello TPL!");
            var sw = new Stopwatch();
            sw.Restart();

            Task<List<int>> task = Primes.FromRangeAsync(7, 7, CancellationToken.None);
            List<int> primes = task.Result;

            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Found {primes.Count} primes after {elapsed}");
        }
    }
}

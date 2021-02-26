using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example8
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hello Async!");

            var task1 = FromRangeAsync(1, 100_000);
            var task2 = FromRangeAsync(100_000, 150_000);
            var task3 = FromRangeAsync(150_000, 200_000);

            var results = await Task.WhenAll(task1, task2, task3);
            var primes = results.Sum(r => r.Count);

            Console.WriteLine($"Found {primes} primes");
        }

        private static async Task<List<int>> FromRangeAsync(int from, int to)
        {
            Console.WriteLine($"1.Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000).ConfigureAwait(false);
            
            Console.WriteLine($"2.Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);

            Console.WriteLine($"3.Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            var result = await Primes.FromRangeAsync(from, to, CancellationToken.None);

            Console.WriteLine($"4.Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }
    }
}

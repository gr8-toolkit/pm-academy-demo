using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello TPL!");
            var sw = new Stopwatch();
            sw.Restart();

            Task<List<int>> task1 = Task.Run(() => Primes.FromRange(1, 100_000));
            Task<List<int>> task2 = Task.Run(() => Primes.FromRange(100_000, 150_000));
            Task<List<int>> task3 = Task.Run(() => Primes.FromRange(150_000, 200_000));
            
            Task.WaitAll(task1, task2, task3);
            int primesCount = task1.Result.Count + task2.Result.Count + task3.Result.Count;

            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Found {primesCount} primes after {elapsed}");
        }
    }
}

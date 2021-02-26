using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example3
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello TPL!");
            var sw = new Stopwatch();
            sw.Restart();

            Task<List<int>> task = Task.Run(() => Primes.FromRange(1, 200_000));
            Task<int> contTask = task.ContinueWith(prevTask =>
            {
                var count = prevTask.Result.Count;
                Console.WriteLine($"Found {count} primes");
                return count;
            });

            int primesCount = contTask.Result;

            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Found {primesCount} primes after {elapsed}");
        }
    }
}

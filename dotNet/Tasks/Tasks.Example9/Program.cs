using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example9
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Hello Async!");
            var sw = new Stopwatch();
            sw.Restart();

            var cts = new CancellationTokenSource(5000);
            var task = Primes.FromRangeAsync(1, 200_000, cts.Token);

            try
            {
                var primes = await task;
                sw.Stop();
                var elapsed = sw.Elapsed;
                Console.WriteLine($"Found {primes.Count} primes after {elapsed}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception '{e}'.");
                // ! Exception property is null for cancelled task !
                Console.WriteLine($"Task exception '{task.Exception}'.");
                Console.WriteLine($"Task status {task.Status}.");
            }

            cts.Dispose();
        }
    }
}

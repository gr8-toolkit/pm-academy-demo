using System;
using System.Diagnostics;
using System.Threading;
using Tasks.Shared;

namespace Tasks.Example4
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello TPL!");
            var sw = new Stopwatch();
            sw.Restart();

            var cts = new CancellationTokenSource(5000);
            var task = Primes.FromRangeAsync(1, 200_000, cts.Token);
            
            try
            {
                var primes = task.Result;
                sw.Stop();
                var elapsed = sw.Elapsed;
                Console.WriteLine($"Found {primes.Count} primes after {elapsed}");
            }
            catch (Exception e)
            {
                // ! Exception property is null for cancelled task !
                Console.WriteLine($"Exception '{e}'.");
                Console.WriteLine($"Task exception '{task.Exception}'." ); 
                Console.WriteLine($"Task status {task.Status}.");
            }

            cts.Dispose();
        }
    }
}

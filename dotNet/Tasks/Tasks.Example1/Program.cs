using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello TPL!");
            var sw = new Stopwatch();
            sw.Restart();
            
            var task = Task.Run(() => Primes.FromRange(1, 200_000));
            task.Wait();
            var primes = task.Result;
            
            sw.Stop();
            var elapsed = sw.Elapsed;
            Console.WriteLine($"Found {primes.Count} primes after {elapsed}");
        }
    }
}

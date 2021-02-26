using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Shared;

namespace Tasks.Example11
{
    class Program
    {
        private const int TaskCount = 10; 
        private static readonly CountdownEvent Countdown = new CountdownEvent(TaskCount);
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Async!");
            var tasks = Enumerable
                .Range(0, TaskCount)
                .Select(_ => WorkloadAsync())
                .ToArray();

            Thread.Sleep(100);
            ThreadPool.GetAvailableThreads(out var workers, out var io);
            Console.WriteLine($"Threads : workers {workers}, io {io}");

            Countdown.Wait();

            Thread.Sleep(1000);
            ThreadPool.GetAvailableThreads(out workers, out io);
            Console.WriteLine($"Threads : workers {workers}, io {io}");

            Task.WaitAll(tasks);
        }

        private static async Task WorkloadAsync()
        {
            await Primes.FromRangeAsync(1, 100_000, CancellationToken.None);
            Countdown.Signal();
            await Task.Delay(10_000);
            Primes.FromRange(1, 1_000);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

#nullable enable

namespace ConcurrentQueueExamples.Examples
{
    public static class ConcurrentQueueExample
    {
        static int _counter = 0;
        const int capacity = 100;
        const int batchSize = 10;
        const int workersCount = 1;

        static ConcurrentQueue<string?> _storage = new ConcurrentQueue<string?>();

        public static async Task Execute()
        {
            try
            {
                await Task.WhenAll(Enumerable.Range(1, workersCount).Select(v => EnqueueAsync()));
                await ReadAllAsync();
                await DequeueAllAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static Task EnqueueAsync() => Task.Run(EnqueueInternal);

        static Task DequeueAllAsync() => Task.Run(DequeueInternal);
        static Task ReadAllAsync() => Task.Run(ReadDataInternal);

        static void EnqueueInternal()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < batchSize; i++)
            {
                _storage.Enqueue(i.ToString());
                Interlocked.Increment(ref _counter);
                sb.Append($"{i}, ");
            }
            Console.WriteLine($"Added data: {sb} count: {_counter}");
        }

        static void DequeueInternal()
        {
            var sb = new StringBuilder();
            while (_storage.TryDequeue(out string? value))
            {
                Interlocked.Decrement(ref _counter);
                sb.Append($"{value}, ");
            }
            Console.WriteLine($"Take result: {sb} count: {_counter}");
        }

        static void ReadDataInternal()
        {
            var sb = new StringBuilder();
            var enumerator = _storage.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sb.Append($"{enumerator.Current}, ");
            }
            Console.WriteLine($"Peek result: {sb} count: {_counter}");
        }
    }
}

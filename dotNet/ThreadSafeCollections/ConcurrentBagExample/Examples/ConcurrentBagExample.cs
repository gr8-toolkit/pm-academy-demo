using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

#nullable enable

namespace ConcurrentBagExamples.Examples
{
    public static class ConcurrentBagExample
    {
        static int _counter = 0;
        const int capacity = 100;
        const int batchSize = 10;
        const int workersCount = 10;

        static ConcurrentBag<string?> _storage = new ConcurrentBag<string?>();
        public static async Task Execute()
        {
            try
            {
                await Task.WhenAll(Enumerable.Range(1, workersCount).Select(v => InitDataAsync()));
                await ReadAllAsync();
                await TakeAllAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static Task InitDataAsync() => Task.Run(AddDataInternal);

        static Task TakeAllAsync() => Task.Run(TakeDataInternal);
        
        static Task ReadAllAsync() => Task.Run(ReadDataInternal);

        static void AddDataInternal()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < batchSize; i++)
            {
                _storage.Add(i.ToString());
                Interlocked.Increment(ref _counter);
                sb.Append($"{i}, ");
            }
            Console.WriteLine($"Added data: {sb} count: {_counter}");
        }

        static void TakeDataInternal()
        {
            var sb = new StringBuilder();
            while (_storage.TryTake(out string? value))
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

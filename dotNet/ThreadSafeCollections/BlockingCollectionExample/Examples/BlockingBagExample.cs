using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

#nullable enable

namespace BlockingCollectionsExamples.Examples
{
    public static class BlockingBagExample
    {
        const int capacity = 100;
        const int batchSize = 10;
        static int _counter = 0;

        static BlockingCollection<string?> _storage = new BlockingCollection<string?>(new ConcurrentBag<string?>(), capacity);

        public static async Task Execute()
        {
            try
            {
                //await InitDataAsync();
                await Task.WhenAll(Enumerable.Range(1, 5).Select(v => InitDataAsync()));
                await TakeAllAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _storage.Dispose();
            }
        }

        static Task InitDataAsync() => Task.Run(AddDataInternal);

        static Task TakeAllAsync() => Task.Run(TakeDataInternal);

        static void AddDataInternal()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < batchSize; i++)
            {
                _storage.Add(i.ToString());
                Interlocked.Increment(ref _counter);
                sb.Append($"{i}, ");
            }
            Console.WriteLine($"Produced data: {sb} count: {_counter}");
        }

        static void TakeDataInternal()
        {
            var sb = new StringBuilder();
            while (_storage.TryTake(out string? value))
            {
                Interlocked.Decrement(ref _counter);
                sb.Append($"{value}, ");
            }
            Console.WriteLine($"Consumed data: {sb} count: {_counter}");
        }
    }
}

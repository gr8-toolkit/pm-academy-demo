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
    public static class BlockingCollectionExample
    {
        const int capacity = 100;
        const int workers = 100;

        static int _counter = 0;
        static BlockingCollection<string?> _storage = new BlockingCollection<string?>(capacity); // ConcurrentQueue on defalut

        public static async Task Execute()
        {
            try
            {
                //await Task.WhenAll(Enumerable.Range(1, 1).Select(v => InitDataAsync()));
                //await TakeAllAsync();

                // simulate producer-consumer
                await Task.WhenAll(Enumerable.Range(1, workers).Select(v => v % 50 != 0 ? InitDataAsync() : TakeAllAsync()));

                //_storage.CompleteAdding();
                //await InitDataAsync(); // add extra data >> The collection has been marked as complete with regards to additions.
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
            for (var i = 0; i <= 5; i++)
            {
                _storage.Add(i.ToString());
                Interlocked.Increment(ref _counter);
                sb.Append($"{i}, ");
            }
            Console.WriteLine($"Produced data: {sb} count: {_counter}");
        }

        static void TakeDataInternal()
        {
            Thread.Sleep(10000);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ConcurrentDictionaryExamples.Examples
{
    public static class ConcurrentDictionaryExample
    {
        const int max = 100_000;
        const int paralels = 10;
        static ConcurrentDictionary<string, int> _storage;

        public static async Task ExecuteAsync()
        {
            await InitDataAsync();
            await Task.WhenAll(Enumerable.Range(1, paralels).Select(v => ProcessData()));
            await CheckData();
        }

        static Task InitDataAsync()
        {
            _storage = new ConcurrentDictionary<string, int>();
            return Task.Run(() =>
            {
                foreach (var i in Enumerable.Range(1, max))
                {
                    _storage.TryAdd(i.ToString(), i - paralels);
                }
            });
        }

        static Task ProcessData()
        {
            return Task.Run(() =>
            {
                foreach (var key in _storage.Keys)
                {
                    _storage.AddOrUpdate(key, 0, (k, v) => v + 1); // 0 => invalidate value
                }
            });
        }

        static Task CheckData()
        {
            return Task.Run(() =>
            {
                var count = 0;
                foreach (var key in _storage.Keys)
                {
                    var intKey = int.Parse(key);
                    var value = _storage[key];
                    count += intKey == value ? 0 : 1;
                }
                Console.WriteLine($"ConcurrentDictionaryExample invalid data count: {count}");
            });
        }

        static void AddOrIncrement(string key)
        {
            _storage.AddOrUpdate(key, 1, (k, v) => v + 1);
        }

        static bool TryUpdate(string key, int oldValue, int newValue)
        {
            return _storage.TryUpdate(key, newValue, oldValue);
        }

        static int GetOrAdd(string key, int newValue)
        {
            return _storage.GetOrAdd(key, v => newValue);
        }
    }
}

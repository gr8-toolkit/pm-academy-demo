using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentDictionaryExamples.Examples
{
    public static class GenericDictionaryExample
    {
        const int _max = 100_000;
        const int _paralels = 10;
        static object _procLock = new object();
        static Dictionary<string, int> _storage = new Dictionary<string, int>();

        public static async Task ExecuteAsync()
        {
            await InitDataAsync();
            await Task.WhenAll(Enumerable.Range(1, _paralels).Select(v => ProcessData()));
            await CheckData();
        }

        private static Task InitDataAsync()
        {
            return Task.Run(() =>
            {
                _storage = Enumerable.Range(1, _max).ToDictionary(v => v.ToString(), y => y - _paralels);
            });
        }

        private static Task ProcessData()
        {
            return Task.Run(() =>
            {
                lock (_procLock)
                {
                    foreach (var key in _storage.Keys)
                    {
                        if (_storage.ContainsKey(key))
                        {
                            //_storage[key] += 1;
                            var oldValue = _storage[key];
                            var newValue = oldValue + 1;
                            _storage[key] = newValue;
                        }
                    }
                }
            });
        }

        private static Task CheckData()
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
                Console.WriteLine($"GenericDictionary invalid data count: {count}");
            });
        }

        static void AddOrIncrement(string key)
        {
            if (_storage.ContainsKey(key))
            {
                _storage[key] += 1;
            }
            else
            {
                _storage.Add(key, 1);
            }
        }

        static bool TryUpdate(string key, int oldValue, int newValue)
        {
            if (_storage.ContainsKey(key) && _storage[key] == oldValue)
            {
                _storage[key] = newValue;
                return true;
            }

            return false;
        }

        static int GetOrAdd(string key, int newValue)
        {
            if (_storage.ContainsKey(key))
            {
                return _storage[key];
            }
            else
            {
                _storage.Add(key, newValue);
                return newValue;
            }
        }
    }
}

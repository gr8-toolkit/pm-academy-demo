using System;
using System.Collections.Generic;
using System.Text;

# nullable enable

namespace CollectionsExample
{
    public static class SynchronizedCollectionExample
    {
        static SynchronizedCollection<string?> _storage = new SynchronizedCollection<string?>();

        static void SyncAdd(string? val)
        {
            lock (_storage.SyncRoot)
            {
                _storage.Add(val);
            }
        }

        static void PrintAll()
        {
            lock (_storage.SyncRoot)
            {
                foreach (var item in _storage)
                {
                    Console.WriteLine(item);
                }
            }
        }

        static int Count()
        {
            lock (_storage.SyncRoot)
            {
                return _storage.Count;
            }
        }

    }
}

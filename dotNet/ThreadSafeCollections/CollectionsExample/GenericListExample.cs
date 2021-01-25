using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

// Ceneric collection classes do not provide any thread synchronization; 
// user code must provide all synchronization when items are added or removed 
// on multiple threads concurrently

namespace CollectionsExample
{
    public static class GenericListExample
    {
        static IList<string?> _storage = new List<string?>();
        static object _syncRoot = new object();

        public static void Execute()
        {

        }

        static void SyncAdd(string? val)
        {
            lock (_syncRoot)
            {
                _storage.Add(val);
            }
        }

        static void PrintAll()
        {
            lock (_syncRoot)
            {
                foreach (var item in _storage)
                {
                    Console.WriteLine(item);
                }
            }
        }

        static int Count()
        {
            lock (_syncRoot)
            {
                return _storage.Count;
            }
        }

    }
}

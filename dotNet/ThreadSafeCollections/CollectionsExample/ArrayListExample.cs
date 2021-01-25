using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

# nullable enable

namespace CollectionsExample
{
    public static class ArrayListExample
    {
        static ArrayList _storage = new ArrayList();

        public static void Execute()
        {

        }

        static void Add(object? val)
        {
            lock (_storage.SyncRoot)
            {
                _storage.Add(val);
            }
        }

        // Creates a synchronized wrapper around the ArrayList.
        // The wrapper works by locking the entire collection on every add or remove operation.
        static ArrayList Synchronized()
        {
            return ArrayList.Synchronized(_storage);
        }

        // Enumerating through a collection is intrinsically not a thread-safe procedure.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// based on https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?view=net-5.0

namespace SourceLockingExample
{
    // Allows multiple readers to coexist with a single writer	
    public static class ReaderWriterLockSlimExample
    {
        static readonly Dictionary<int, string> _cache = new Dictionary<int, string>();
        static readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
        static readonly int _count = 100;

        public static void Execute()
        {
            Foo();
        }

        static void Foo()
        {
            var insertTask = Task.Run(() => InitData(_count));
            insertTask.GetAwaiter().GetResult();
            var readTasks = new[]
            {
                Task.Run(() => Read(0, true, _count)),
                Task.Run(() => Read(_count/2, false, _count))
            };
            Task.WaitAll(readTasks);
        }

        static void InitData(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Write(i, $"Value {i}");
            }
        }

        static void Read(int startIndex, bool asc, int count)
        {
            var step = asc ? 1 : -1;
            var maxIndex = asc ? count : 1;

            var sb = new StringBuilder();
            for (int i = startIndex; asc ? i < maxIndex : i >= maxIndex; i += step)
            {
                sb.AppendLine($"{i} {Read(i)}");
            }

            Console.WriteLine(sb.ToString());
        }

        static string Read(int key)
        {
            _lockSlim.EnterReadLock();
            try
            {
                return _cache[key];
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        static void Write(int key, string value)
        {
            _lockSlim.EnterWriteLock();
            try
            {
                _cache.Add(key, value);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        static bool TryWrite(int key, string value, int timeout)
        {
            if (_lockSlim.TryEnterWriteLock(timeout))
            {
                try
                {
                    _cache.Add(key, value);
                }
                finally
                {
                    _lockSlim.ExitWriteLock();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Delete(int key)
        {
            _lockSlim.EnterWriteLock();
            try
            {
                _cache.Remove(key);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }
    }
}

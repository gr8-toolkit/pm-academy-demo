using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SourceLockingExample
{
    // Ensures just one thread can access a resource, or section of code at a time
    public static class MonitorExample
    {
        static readonly object _lock1 = new object();
        static readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(500);
        static int _sharedSource;

        public  static void IncrementWithLock()
        {
            lock (_lock1)
            {
                // critical section
                _sharedSource++;
            }
        }

        static void IncrementWithMonitorM1()
        {
            Monitor.Enter(_lock1);
            // critical section
            _sharedSource++;
            Monitor.Exit(_lock1);
        }

        static void IncrementWithMonitorM2()
        {
            var lockTaken = false;
            try
            {

                lockTaken = Monitor.IsEntered(_lock1);

                Monitor.Enter(_lock1, ref lockTaken);
                if (lockTaken)
                {
                    // critical section
                    _sharedSource++;
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lock1);
                }
            }
        }

        static void IncrementWithMonitorM3()
        {
            var lockTaken = false;
            try
            {
                lockTaken = Monitor.TryEnter(_lock1);
                if (lockTaken)
                {
                    // critical section
                    _sharedSource++;
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lock1);
                }
            }
        }

        static void IncrementWithMonitorM4()
        {
            var lockTaken = false;
            try
            {
                Monitor.TryEnter(_lock1, _timeout, ref lockTaken);
                if (lockTaken)
                {
                    // critical section
                    _sharedSource++;
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_lock1);
                }
            }
        }

        // also see InterlockedExample
    }
}


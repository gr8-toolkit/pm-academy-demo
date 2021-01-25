using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SourceLockingExample
{
    public static class SynchronizationLockExceptionExample
    {
        static object _lock = new object();

        public static void Execute()
        {
            try
            {
                Console.WriteLine("SynchronizationLockException in action");
                Monitor.Exit(_lock); // Object synchronization method was called from an unsynchronized block of code.
                Monitor.Wait(_lock);
                Monitor.Pulse(_lock);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }
    }
}

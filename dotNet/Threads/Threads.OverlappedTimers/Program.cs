using System;
using System.Threading;

namespace Threads.OverlappedTimers
{
    /// <summary>
    /// Timer overlapping demo.
    /// Based on <see cref="Timer"/>.
    /// </summary>
    internal class Program
    {
        private static readonly object Sync = new object();

        static void Main()
        {
            Console.WriteLine("Timer overlapping demo");

            // DEMO : Timer multiple entry issue
            var t = new Timer(Callback, null, 0, 1_000);

            // DEMO : Fix timer multiple entry issue
            //var t = new Timer(SyncCallback, null, 0, 1000);
            
            // Demo duration - 20 secs
            Thread.Sleep(20_000);
            
            // Release timer
            t.Dispose();
            
            Console.WriteLine("Done.");
        }

        private static void SyncCallback(object state)
        {
            try
            {
                // Try to catch Sync marker.
                if (!Monitor.TryEnter(Sync)) return;
                Callback(state);
            }
            finally
            {
                if (Monitor.IsEntered(Sync)) Monitor.Exit(Sync);
            }
        }

        private static void Callback(object _)
        {
            var id = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"[{id}] Enter timer callback at: {DateTime.Now:T} ");
            
            // Some hard work (3 sec)
            SpinWait.SpinUntil(() => false, 3_000);
            
            Console.WriteLine($"[{id}] Leave timer callback");
        }        
    }
}

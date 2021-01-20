using System;
using System.Threading;

namespace Threads.OverlappedTimers
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello Timer!");

            // DEMO : Timer multiple entry issue
            //var t = new Timer(Callback, null, 0, 1_000);

            // DEMO : Fix timer multiple entry issue
            var t = new Timer(SyncCallback, null, 0, 1000);
            
            
            Thread.Sleep(20_000);
            t.Dispose();
            Console.WriteLine("Good buy!");
        }

        private static void Callback(object state)
        {
            var id = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"Time: {DateTime.Now:T} [{id}]");
            SpinWait.SpinUntil(() => false, 3_000);
            Console.WriteLine($"Timer good buy [{id}]");
        }


        private static readonly object Marker = new object();

        private static void SyncCallback(object state)
        {
            try
            {
                if (!Monitor.TryEnter(Marker)) return;
                Callback(state);
            }
            finally
            {
                if (Monitor.IsEntered(Marker)) Monitor.Exit(Marker);
            }
        }
    }
}

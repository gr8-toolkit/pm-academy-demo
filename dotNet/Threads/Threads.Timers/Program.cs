using System;
using System.Threading;

namespace Threads.Timers
{
    class Program
    {
        static void Main()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Hello Timer! [{id}]");
            
            var t = new Timer(Callback, null, 0, 1_000);
            
            Console.WriteLine("Press enter to quit!");
            Console.ReadLine();

            t.Dispose();
            Console.WriteLine("Good buy!");
        }

        private static void Callback(object state)
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"[Now] : {DateTime.Now:O} [{id}]");
        }
    }
}

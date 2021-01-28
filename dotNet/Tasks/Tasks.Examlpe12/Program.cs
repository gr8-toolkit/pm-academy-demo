using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks.Examlpe12
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Async");
            
            var tcs = new TaskCompletionSource<int>();
            var timer = new Timer(state => { tcs.SetResult(0); }, null, 10_000,  Timeout.Infinite);
            await tcs.Task;
            
            timer.Dispose();
            Console.WriteLine("Done");
        }
    }
}

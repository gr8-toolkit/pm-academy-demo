using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationProblems
{
    public static class RaceConditionExample
    {
        private static int _counter;

        public static void ExecuteM1()
        {
            Console.WriteLine("Starting M1 ...");

            var tasks = new Task[100];
            for (int i = 0; i < 100; i++)
            {
                tasks[i] = Task.Run(Increment);
            }

            Task.WaitAll(tasks);
            Console.WriteLine($"Counter: {_counter}");
            Console.WriteLine("Finished M1 ...");
        }

        private static void Increment()
        {
            Thread.Sleep(50);
            _counter++;
        }


        private static void IncrementInt()
        {
            Thread.Sleep(50);
            Interlocked.Increment(ref _counter);
        }
    }
}

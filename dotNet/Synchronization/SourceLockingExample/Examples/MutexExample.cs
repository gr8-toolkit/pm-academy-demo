using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// A Mutex is like a C# lock, but it can work across multiple processes. 
// In other words, Mutex can be computer-wide as well as application-wide.
namespace SourceLockingExample
{
    public static class MutexExample
    {
        static Mutex _mutex = new Mutex();
        //static Mutex _sharedMutex = Mutex.OpenExisting("SourceLockingExample-M1");
        static Mutex _sharedMutex2 = new Mutex(true, "SourceLockingExample-M2");
        static int _attempts = 100;
        static TimeSpan _waitingTimeout = TimeSpan.FromMilliseconds(500);
        static TimeSpan _workingTimeout = TimeSpan.FromMilliseconds(1500);
        static int _sharedSource;

        public static void Execute()
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(new ThreadStart(Foo))
                {
                    Name = String.Format($"Thread{i + 1}")
                };
                thread.Start();
            }
        }

        static void Foo()
        {
            for (int i = 0; i < _attempts; i++)
            {
                Bar();
            }
        }

        static void Bar()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is requesting the mutex");
            if (_mutex.WaitOne(_waitingTimeout))
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} has entered to the critical section");

                Thread.Sleep(_workingTimeout);
                _sharedSource++;
                Console.WriteLine($"{Thread.CurrentThread.Name} is leaving the critical section");

                _mutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} will not acquire the mutex");
            }
        }
    }
}

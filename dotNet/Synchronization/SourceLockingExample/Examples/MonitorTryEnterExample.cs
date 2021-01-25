using System;
using System.Threading;
using System.Threading.Tasks;

namespace SourceLockingExample
{
    public static class MonitorTryEnterExample
    {
        static readonly object _lock1 = new object();
        static readonly object _lock2 = new object();
        static readonly TimeSpan _attemptsTimeout = TimeSpan.FromMilliseconds(100);

        public static void ExecuteM1()
        {
            Console.WriteLine("Starting M1...");
            var foo = new Thread(FooM1);
            var bar = new Thread(BarM1);
            foo.Start();
            bar.Start();
            Console.WriteLine("Finished M1 ...");
        }

        public static void ExecuteM2()
        {
            Console.WriteLine("Starting M2 ...");
            var foo = Task.Run(FooM1);
            var bar = Task.Run(BarM1);
            Task.WaitAll(foo, bar);
            Console.WriteLine("Finished M2 ...");
        }

        static void FooM1()
        {
            Console.WriteLine("Foo enter");
            bool acquiredLock1, acquiredLock2;
            while (true)
            {
                try
                {
                    acquiredLock1 = Monitor.TryEnter(_lock1, _attemptsTimeout);
                    if (acquiredLock1)
                    {
                        Console.WriteLine("working in Foo #1");
                        Thread.Sleep(100);
                        Console.WriteLine("waits for Bar lock in Foo");
                        acquiredLock2 = Monitor.TryEnter(_lock2, _attemptsTimeout);
                        if (acquiredLock2)
                        {
                            Console.WriteLine("working in Foo #2");
                            break;
                        }
                    }
                }
                finally
                {
                    TryMonitorExit();
                }
            }
            Console.WriteLine("Foo exit");
        }

        static void BarM1()
        {
            Console.WriteLine("Bar enter");
            bool acquiredLock1, acquiredLock2;
            while (true)
            {
                try
                {
                    acquiredLock1 = Monitor.TryEnter(_lock2, _attemptsTimeout);
                    if (acquiredLock1)
                    {
                        Console.WriteLine("working in Bar #1");
                        Thread.Sleep(100);
                        Console.WriteLine("waits for Foo lock in Bar");
                        acquiredLock2 = Monitor.TryEnter(_lock1, _attemptsTimeout);
                        if (acquiredLock2)
                        {
                            Console.WriteLine("working in Bar #2");
                            break;
                        }
                    }
                }
                finally
                {
                    TryMonitorExit();
                }
            }
            Console.WriteLine("Bar exit");
        }

        private static void TryMonitorExit()
        {
            if (Monitor.IsEntered(_lock1))
            {
                Monitor.Exit(_lock1);
            }

            if (Monitor.IsEntered(_lock2))
            {
                Monitor.Exit(_lock2);
            }
        }

        private static void Step1(bool running)
        {
            lock (_lock1)
            {
                Console.WriteLine("Enter Step1");

                Thread.Sleep(2000); // some work

                Monitor.Wait(_lock1); // allows to run Step2


                Console.WriteLine("After Wait");
            }
            Console.WriteLine("Exit Step1");
        }

        private static void Step2(bool running)
        {
            lock (_lock1)
            {
                // wait Monitor.Wait(_lock1) in Step1

                Console.WriteLine("Enter Step2");

                Thread.Sleep(2000); // some work

                Monitor.Pulse(_lock1);

                Console.WriteLine("After Pulse");
            }
            Console.WriteLine("Exit Step2");
        }
    }
}

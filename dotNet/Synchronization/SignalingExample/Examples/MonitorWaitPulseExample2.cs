using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// based on http://www.albahari.com/threading/part4.aspx#_Signaling_with_Wait_and_Pulse

namespace SignalingExample
{
    // producer-consumer
    public static class MonitorWaitPulseExample2
    {
        static readonly object _lock = new object();
        static Queue<int> _queue = new Queue<int>();
        static int _workers = 2;

        public static void Execute()
        {
            var consumers = new Thread[_workers];

            for (int i = 0; i < _workers; i++)
            {
                consumers[i] = new Thread(Bar)
                {
                    Name = $"Worker {i}"
                };
                consumers[i].Start();
            }

            Console.WriteLine("Enqueuing 10 items...");

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Foo(i);
            }

            // break infinity cycle
            for (int i = 0; i < _workers; i++)
            {
                Foo(int.MinValue);
                consumers[i].Join();
            }
        }

        static void Foo(int val) // Enqueue
        {
            Console.WriteLine($"Adds {val} to queue");
            lock (_lock)
            {
                _queue.Enqueue(val);
                Monitor.Pulse(_lock); // allow to consume
            }
        }

        static void Bar() // Consume
        {
            Console.WriteLine($"Start consume in {Thread.CurrentThread.Name}");

            int val = int.MinValue;
            while (true) // keep consuming
            {
                lock (_lock)
                {
                    while (_queue.Count == 0)
                    {
                        Monitor.Wait(_lock);
                    }
                    val = _queue.Dequeue();
                }
                if (val == int.MinValue)
                {
                    Console.WriteLine($"Stop consume in {Thread.CurrentThread.Name}");
                    return;
                };
                Console.WriteLine($"Show item from queue: {val} in {Thread.CurrentThread.Name}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Start Foo
//Start Bar 2
//Start Bar 0
//Start Bar 1
//Start Bar 3
//Start Bar 4
//Waining for Bar 5 workers
//Send signal from 0
//Send signal from 2
//End Bar 2
//End Bar 0
//Send signal from 1
//End Bar 1
//Send signal from 3
//End Bar 3
//Send signal from 4
//End Bar 4
//All threads have finished working

namespace SignalingExample
{
    public static class CountdownEventExample
    {
        static int _count = 5;
        static CountdownEvent _countdown = new CountdownEvent(_count);

        public static void Execute()
        {
            Foo();
        }

        static void Foo()
        {
            Console.WriteLine($"Start Foo {Thread.CurrentThread.Name}");
            var workers = new Thread[_count];
            for (int i = 0; i < _count; i++)
            {
                workers[i] = new Thread(Bar)
                {
                    Name = i.ToString()
                };
                workers[i].Start();
            }
            Console.WriteLine($"Waining for Bar {_count} workers");
            _countdown.Wait();
            Console.WriteLine("All threads have finished working");
        }

        static void Bar()
        {
            Console.WriteLine($"Start Bar {Thread.CurrentThread.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"Send signal from {Thread.CurrentThread.Name}");

            _countdown.Signal();

            Console.WriteLine($"End Bar {Thread.CurrentThread.Name}");
        }
    }
}

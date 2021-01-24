using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Console output:
//send signal to start and wait 1s
//Foo: initialize work
//Bar: initialize work
//send signal to pause 5s
//Foo: Working 1
//Bar: Working 1
//send signal to continue and pause 4s
//Bar: Working 2
//Foo: Working 2
//send signal to finish
//Bar: All done
//Foo: All done

namespace SignalingExample
{
    public static class ManualResetEventExample
    {
        static readonly ManualResetEvent _ewh = new ManualResetEvent(false);

        public static void Execute()
        {
            var foo = new Thread(Foo);
            var bar = new Thread(Bar);
            foo.Start();
            bar.Start();

            Console.WriteLine("send signal to start");
            _ewh.Set();
            Console.WriteLine("send signal to pause 5s");
            _ewh.Reset(); // need to call reset on ManualResetEvent
            Thread.Sleep(1000);

            Console.WriteLine("send signal to continue and pause 4s");
            _ewh.Set();
            _ewh.Reset();
            Thread.Sleep(4000);

            Console.WriteLine("send signal to finish");
            _ewh.Set();
        }

        static void Foo()
        {
            InnerWork("Foo");
        }

        static void Bar()
        {
            InnerWork("Bar");
        }

        static void InnerWork(string name)
        {
            Console.WriteLine($"{name}: initialize work");
            _ewh.WaitOne();

            Thread.Sleep(2000);

            Console.WriteLine($"{name}: Working 1");
            _ewh.WaitOne();

            Thread.Sleep(1000);

            Console.WriteLine($"{name}: Working 2");
            _ewh.WaitOne();

            Console.WriteLine($"{name}: All done");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Console output:
//Start Bar
//Start Foo
//Bar unblocks Foo [1]
//Continue Foo[1]
//Bar unblocks Foo [2]
//Exit Bar
//Continue Foo [2]
//Exit Foo


namespace SignalingExample
{
    public static class AutoResetEventExample
    {
        static readonly AutoResetEvent _ewh = new AutoResetEvent(false);

        public static void Execute()
        {
            var foo = new Thread(Foo);
            var bar = new Thread(Bar);
            foo.Start();
            bar.Start();
        }


        static void Foo()
        {
            Console.WriteLine("Start Foo and wait for signal");
            _ewh.WaitOne();
            // wait 1s
            Console.WriteLine("Continue Foo [1]");
            _ewh.WaitOne();
            // wait 5s
            Console.WriteLine("Continue Foo [2]");
            Console.WriteLine("Exit Foo");
        }

        static void Bar()
        {
            Console.WriteLine("Start Bar");
            Thread.Sleep(1000);
            Console.WriteLine("Bar unblocks Foo [1]");
            _ewh.Set();
            Thread.Sleep(5000);
            Console.WriteLine("Bar unblocks Foo [2]");
            _ewh.Set();
            Console.WriteLine("Exit Bar");
        }
    }
}






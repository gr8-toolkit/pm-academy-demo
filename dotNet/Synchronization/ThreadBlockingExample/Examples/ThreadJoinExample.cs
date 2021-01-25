using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadBlockingExample
{
    public static class ThreadJoinExample
    {
        public const int JOIN_TIMEOUT = 1000;
        public const int WORK_TIMEOUT = 200;

        public static void ExecuteM1()
        {
            Console.WriteLine("Starting M1 ...");

            var foo = new Thread(Foo1);
            foo.Start();

            Console.WriteLine("Finished M1 ...");
        }


        public static void Foo1()
        {
            Console.WriteLine("Foo1 enter");
            Thread work = new Thread(Bar);
            work.Start();
            work.Join(); // blocks the calling thread
            Console.WriteLine("End1 of Foo");
        }

        public static void Foo2()
        {
            Console.WriteLine("Foo2 enter");
            Thread work = new Thread(Bar);
            work.Start();
            work.Join(JOIN_TIMEOUT); // blocks the calling thread on JOIN_TIMEOUT
            Console.WriteLine("End of Foo2");
        }


        private static void Bar(object obj)
        {
            Console.WriteLine("Bar enter");
            Thread.Sleep(WORK_TIMEOUT); // simulate work
            Console.WriteLine("End of Bar");
        }
    }
}

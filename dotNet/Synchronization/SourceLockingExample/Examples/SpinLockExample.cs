using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceLockingExample
{
    public static class SpinLockExample
    {
        private static readonly SpinLock _spinLock = new SpinLock();
        private static readonly Queue<SampleClass> _queue = new Queue<SampleClass>();
        private static readonly int _count = 1000;

        public static void Execute()
        {
            var foo = new Thread(Foo);
            foo.Start();
        }

        static void Foo()
        {
            for (int i = 0; i < _count; i++)
            {
                Bar(new SampleClass() { Foo = i.ToString() });
            }
        }

        static void Bar(SampleClass m)
        {
            bool lockTaken = false;
            try
            {
                _spinLock.Enter(ref lockTaken);
                _queue.Enqueue(m);
            }
            finally
            {
                if (lockTaken) _spinLock.Exit(false);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceLockingExample
{
    public static class MutexInstanceLockExample
    {
        static Mutex _mutex = new Mutex(true, "MutexInstanceLock");

        public static void Execute()
        {
            if (Foo())
            {
                Console.WriteLine("new instance");
            }
            else
            {
                Console.WriteLine("already created");
            }
        }

        static bool Foo()
        {
            if(!_mutex.WaitOne(2000, false))
            {
                return false;
            }
            return true;
        }
    }
}

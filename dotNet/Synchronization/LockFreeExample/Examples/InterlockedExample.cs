using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LockFreeExample
{
    public static class InterlockedExample
    {
        static int _sharedSource;

        public static void ExecuteM1()
        {
            var lf = Task.Run(LockFreeOperations);
            lf.GetAwaiter().GetResult();
        }


        static void LockFreeIncrement()
        {
            Interlocked.Increment(ref _sharedSource);
        }

        static void LockFreeOperations()
        {
            var ss1 = Interlocked.Add(ref _sharedSource, 100);                      //  100,100
            var ss2 = Interlocked.Decrement(ref _sharedSource);                     //  99,99
            var ss3 = Interlocked.Exchange(ref _sharedSource, 123);                 // 99,123

            var ss4 = Interlocked.CompareExchange(ref _sharedSource, 456, 123);     //  123,456
            
            
            var ss5 = Interlocked.CompareExchange(ref _sharedSource, 111, 123);     //  456,456
        }

    }
}

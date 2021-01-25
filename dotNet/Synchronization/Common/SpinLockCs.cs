using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class SpinLockCs
    {
        private volatile int _locked;

        public void Acquire()
        {
            while (Interlocked.CompareExchange(ref _locked, 1, 0) != 0) ;
        }

        public void Release()
        {
            _locked = 0;
        }
    }
}

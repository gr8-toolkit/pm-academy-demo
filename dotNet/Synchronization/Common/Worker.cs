using System;
using System.Threading;

namespace Common
{
    public class Worker
    {
        private volatile bool _shouldStop;

        public void Execute()
        {
            var working = false;
            while (!_shouldStop)
            {
                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            _shouldStop = false;
        }
    }
}

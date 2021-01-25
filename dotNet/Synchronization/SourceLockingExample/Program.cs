using System;

namespace SourceLockingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //MutexInstanceLockExample.Execute();
            //MonitorExample.IncrementWithLock();
            //MonitorTryEnterExample.ExecuteM1();
            //MonitorTryEnterExample.ExecuteM2();
            //MutexExample.Execute();
            //SpinLockExample.Execute();
            //SpinWaitExample.Execute();
            //SemaphoreExample.Execute();

            ReaderWriterLockSlimExample.Execute();

            //using SemaphoreSlimExample sse = new SemaphoreSlimExample();
            //sse.Execute();

            //SemaphoreSlimExample2.Execute();
            //SemaphoreSlimExample3.Execute();
            //SynchronizationLockExceptionExample.Execute();
            Console.ReadKey();
        }
    }
}

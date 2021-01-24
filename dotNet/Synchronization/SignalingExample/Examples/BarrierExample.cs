using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace SignalingExample
{
    public static class BarrierExample
    {
        static int _participantsCount = 7;
        static Barrier _barrierM1 = new Barrier(_participantsCount);
        static Barrier _barrierM2 = new Barrier(0);

        public static void Execute()
        {
            _barrierM2.AddParticipants(_participantsCount);
            _barrierM2.AddParticipant();
            _barrierM2.RemoveParticipant();

            var participants = new Thread[_participantsCount];

            for (var i = 0; i < _participantsCount; i++)
            {
                participants[i] = new Thread(Foo)
                {
                    Name = i.ToString()
                };
                participants[i].Start();
            }
        }

        static void Foo()
        {
            Step1();
            Step2();
            Step3();
        }

        static void Step1()
        {
            Console.WriteLine($"Execute step1 in thread {Thread.CurrentThread.Name}");
            Thread.Sleep(500);
            Console.WriteLine($"Finished step1 in thread {Thread.CurrentThread.Name}");
            _barrierM1.SignalAndWait();
        }

        static void Step2()
        {
            Console.WriteLine($"Execute step2 in thread {Thread.CurrentThread.Name}");
            Thread.Sleep(1500);
            Console.WriteLine($"Finished step2 in thread {Thread.CurrentThread.Name}");
            _barrierM1.SignalAndWait();
        }

        static void Step3()
        {
            Console.WriteLine($"Execute step3 in thread {Thread.CurrentThread.Name}");
            Thread.Sleep(300);
            Console.WriteLine($"Finished step3 in thread {Thread.CurrentThread.Name}");
            _barrierM1.SignalAndWait();
        }
    }
}
//Execute step1 in thread 1
//Execute step1 in thread 0
//Execute step1 in thread 2
//Execute step1 in thread 3
//Execute step1 in thread 4
//Finished step1 in thread 1
//Finished step1 in thread 4
//Finished step1 in thread 3
//Finished step1 in thread 0
//Finished step1 in thread 2
//Execute step2 in thread 2
//Execute step2 in thread 4
//Execute step2 in thread 3
//Execute step2 in thread 0
//Execute step2 in thread 1
//Finished step2 in thread 1
//Finished step2 in thread 0
//Finished step2 in thread 2
//Finished step2 in thread 3
//Finished step2 in thread 4
//Execute step3 in thread 4
//Execute step3 in thread 2
//Execute step3 in thread 1
//Execute step3 in thread 0
//Execute step3 in thread 3
//Finished step3 in thread 4
//Finished step3 in thread 2
//Finished step3 in thread 3
//Finished step3 in thread 1
//Finished step3 in thread 0

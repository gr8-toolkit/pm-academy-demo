using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// source: https://www.c-sharpcorner.com/UploadFile/1d42da/wait-and-pulse-method-in-threading-C-Sharp/

namespace SignalingExample
{
    public static class MonitorWaitPulseExample
    {
        public static void Execute()
        {
            PingPong pp = new PingPong();
            Console.WriteLine("The Ball is dropped... \n");
            ThreadWrapper mythread1 = new ThreadWrapper("Ping", pp);
            ThreadWrapper mythread2 = new ThreadWrapper("Pong", pp);
            mythread1.thread.Join();
            mythread2.thread.Join();
            Console.WriteLine("\nThe Ball Stops Bouncing.");
        }
    }


    class PingPong
    {
        public void Ping(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    //ball halts.
                    Monitor.Pulse(this); // notify any waiting threads
                    return;
                }
                Console.Write("Ping ");
                Monitor.Pulse(this); // let Pong() run
                Monitor.Wait(this); // wait for pong() to complete
            }
        }

        public void Pong(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    //ball halts.
                    Monitor.Pulse(this); // notify any waiting threads
                    return;
                }
                Console.WriteLine("Pong ");
                Monitor.Pulse(this); // let Ping() run
                Monitor.Wait(this); // wait for ping() to complete
            }
        }
    }

    class ThreadWrapper
    {
        public Thread thread;
        PingPong pp;

        public ThreadWrapper(string name, PingPong ppObject)
        {
            thread = new Thread(new ThreadStart(this.run));
            pp = ppObject;
            thread.Name = name;
            thread.Start();
        }

        //Begin execution of new thread.
        void run()
        {
            if (thread.Name == "Ping")
            {
                for (int i = 0; i < 5; i++)
                    pp.Ping(true);
                pp.Ping(false);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                    pp.Pong(true);
                pp.Pong(false);
            }
        }
    }
}

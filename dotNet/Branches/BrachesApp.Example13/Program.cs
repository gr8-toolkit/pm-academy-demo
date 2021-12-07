using System;
using System.Collections.Generic;
using System.Threading;

namespace BrachesApp.Example13
{

    class Program
    {
        static Dictionary<(int, int, char), string> results = new Dictionary<(int, int, char), string>();

        static void Main(string[] args)
        {
            var a = 13;
            var b = 14;
            var operation = '*';

            for(int i = 0; i < 5; i++)
            {
                DoSomething(a, b, operation);
            }
        }

        public static void DoSomething(int a, int b, char opt)
        {
            if (results.TryGetValue((a, b, opt), out string res))
            {
                //Get some condition result from cache
                Console.WriteLine(res);
            }
            else if (Calc(a, b, opt) > 130)
            {
                //Get result and save it
                Console.WriteLine("Very big number");
                results.Add((a, b, opt), "Very big number");
            }
            else if (Calc(a, b, opt) < 130)
            {
                //Get result and save it
                Console.WriteLine("Very small number");
                results.Add((a, b, opt), "Very small number");
            }
            else if (Calc(a, b, opt) == 130)
            {
                //Get result and save it
                Console.WriteLine("Exact number");
                results.Add((a, b, opt), "Exact number");
            }
        }

        //Long Running
        public static int Calc(int a, int b, char opt)
        {
            Thread.Sleep(10000);
            return opt switch
            {
                '*' => a * b,
                '+' => a + b,
                '-' => a - b,
                '/' when b != 0 => a / b,
                _ => 0
            };
        }
    }
}

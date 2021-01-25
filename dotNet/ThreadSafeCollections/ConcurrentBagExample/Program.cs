using ConcurrentBagExamples.Examples;
using System;
using System.Threading.Tasks;

namespace ConcurrentBagExamples
{
    class Program
    {
        async static Task Main(string[] args)
        {
            await ConcurrentBagExample.Execute();
            Console.ReadKey();
        }
    }
}

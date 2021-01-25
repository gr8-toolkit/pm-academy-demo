using ConcurrentStackExamples.Examples;
using System;
using System.Threading.Tasks;

namespace ConcurrentStackExamples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ConcurrentStackExample.Execute();
            Console.ReadKey();
        }
    }
}

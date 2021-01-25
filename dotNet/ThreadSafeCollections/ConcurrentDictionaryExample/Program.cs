using ConcurrentDictionaryExamples.Examples;
using System;
using System.Threading.Tasks;

namespace ConcurrentDictionaryExamples
{
    class Program
    {
        async static Task Main(string[] args)
        {
            await GenericDictionaryExample.ExecuteAsync();
            await ConcurrentDictionaryExample.ExecuteAsync();
            Console.ReadKey();
        }
    }
}

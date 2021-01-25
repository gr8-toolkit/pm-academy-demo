using BlockingCollectionsExamples.Examples;
using System;
using System.Threading.Tasks;

namespace BlockingCollectionsExamples
{
    class Program
    {
        async static Task Main(string[] args)
        {
            await BlockingCollectionExample.Execute();
            //await BlockingQueueExample.Execute();
            //await BlockingStackExample.Execute();
            //await BlockingBagExample.Execute();
            Console.ReadKey();
        }
    }
}

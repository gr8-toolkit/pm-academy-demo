using Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// https://devblogs.microsoft.com/pfxteam/when-to-use-parallel-foreach-and-when-to-use-plinq/

namespace PLinqExample
{
    class Program
    {

        const int max = 100_000;
        static void Main(string[] args)
        {
            //AsParallel();
            //ForAll();
            //AsOrdered();
            //WithCancellation();
            Console.ReadKey();
        }


        static void AsParallel()
        {
            int[] numbers = Enumerable.Range(1, max).ToArray();
            var factorials = from n in numbers.AsParallel() select Factorial(n);
            foreach (var n in factorials)
                Console.WriteLine(n);
        }

        static void ForAll()
        {
            int[] numbers = Enumerable.Range(1, max).ToArray();
            (from n in numbers.AsParallel() select Factorial(n)).ForAll(Console.WriteLine);
        }

        static void AsOrdered()
        {
            int[] numbers = Enumerable.Range(1, max).ToArray();

            var factorials = from n in numbers.AsParallel().AsOrdered() select Factorial(n);
        }

        static void WithCancellation()
        {
            var source = Enumerable.Range(1, 10000);
            using var cts = new CancellationTokenSource();

            try
            {
                // Sets the degree of parallelism to use in a query. Degree of parallelism is the
                // maximum number of concurrently executing tasks that will be used to process the
                // query.
                var evenNums = from num in source
                      .AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .WithCancellation(cts.Token)
                               where num % 2 == 0
                               select num;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions != null)
                {
                    foreach (var ie in e.InnerExceptions)
                    {
                        Console.WriteLine(ie.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Streaming data-parralel operation
        static void AnalyseLogs(IEnumerable<LogItem> logs)
        {
            // stream of results
            var heavyWarnings = logs.AsParallel().AsOrdered().Select(
                log => new { log = log, weight = log.Message.Length }).
                Where(v => v.weight > 100 && (int)v.log.Level > (int)(LogSeverity.Info));

            foreach (var warning in heavyWarnings)
            {
                ProcessLogsWeight(warning.weight);
            }
        }

        static void ZippingLogs(IEnumerable<LogItem> logs1, IEnumerable<LogItem> logs2)
        {
            //  each data source can be processed concurrently with different computations
            // results could be combined with Zip operator
            var result = logs1
                .AsParallel()
                .AsOrdered()
                .Select(v => ProcessLog(v))
                .Zip(logs2
                .AsParallel()
                .AsOrdered()
                .Select(v => ProcessLog(v)),
                (first, second) => Combine(first, second));
        }

        static int Factorial(int x)
        {
            int result = 1;

            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }
            Console.WriteLine($"F({x})= {result}");
            return result;
        }

        static void ProcessLogsWeight(int weight)
        {
            Thread.Sleep(50);
        }

        static LogItem ProcessLog(LogItem log)
        {
            Thread.Sleep(50);
            return log;
        }

        static LogItem Combine(LogItem log1, LogItem log2)
        {
            Thread.Sleep(50);
            return log1;
        }
    }
}

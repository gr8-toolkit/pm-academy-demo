using Common.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Allows to iterate over an enumerable data set.
namespace ParallelExamples
{
    class Program
    {
        static int _counter = 1000;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(3, 4);

        static void Main(string[] args)
        {
            Execute();
            Console.ReadKey();
        }

        static void Execute()
        {
            //ParallelInvoke();
            //ParralelFor();
            //ParallelForeach();
        }

        static void ParallelFor()
        {
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var loopResult1 = Parallel.For(0, _counter, parallelOptions, TestAction);

            var loopResult2 = Parallel.For(0, _counter, parallelOptions, delegate (int i)
            {
                Console.WriteLine($"Delegate #{i + 1}, Thread #{Thread.CurrentThread.ManagedThreadId}");
            });

            var loopResult3 = Parallel.For(0, _counter, parallelOptions, i =>
            {
                Console.WriteLine($"Lambda #{i + 1}, Thread #{Thread.CurrentThread.ManagedThreadId}");
            });
        }

        static void ParallelForeach()// with thread local state
        {
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,

            };

            int result1 = 0, result2 = 0, localInitCounter = 0, bodyCounter = 0, localFinallyCounter = 0;
            // An enumerable data source.
            int[] source = { 1, 2, 3, 4, 5, 6, 7, 88, 42, 35, 9, 967, 123, 453, 190, 13, 12, 42 };
            //source = Enumerable.Range(1, 10_000).ToArray();


            // The localInit delegate is invoked once for each task that participates in the loop's execution and returns the initial local state for each of those tasks. 
            // These initial states are passed to the first body invocations on each task. 
            int localInit()
            {
                Interlocked.Increment(ref localInitCounter);
                //Console.WriteLine($"Calls localInit #{localInitCounter} in Thread={Thread.CurrentThread.ManagedThreadId}");
                return 0;
            };

            // The body delegate is invoked once for each element in the source enumerable. 
            int body(int val, ParallelLoopState loopState, int localSum)
            {
                if (val == 10000)
                {
                    loopState.Stop();  // exiting from operation
                }


                // you can also filter {val} before adding
                localSum += val;
                Interlocked.Increment(ref bodyCounter);
                //Console.WriteLine($"Calls Body #{bodyCounter}: number={val}, localSum={localSum}, Thread={Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(50); // simulate some work on each increment
                return localSum;
            }

            // Last body invocation on each task returns a state value that is passed to the localFinally delegate.
            // The localFinally delegate is invoked once per thread to perform a final action on each task's local state.
            void localFinally(int localSum)
            {
                Interlocked.Increment(ref localFinallyCounter);
                //Console.WriteLine($"Calls localFinally #{localFinallyCounter} in Thread={Thread.CurrentThread.ManagedThreadId}");
                Interlocked.Add(ref result1, localSum);
            };

            var stopWatch1 = Stopwatch.StartNew();
            var loopResult = Parallel.ForEach(source, parallelOptions, localInit, body, localFinally);
            stopWatch1.Stop();

            var stopWatch2 = Stopwatch.StartNew();
            //result2 = Enumerable.Sum(source)
            foreach (var num in source)
            {
                Thread.Sleep(50); // simulate some work on each increment
                result2 += num;
            };
            stopWatch2.Stop();

            Console.WriteLine($"parralel results: sum= {result1}, elapsed time= {stopWatch1.ElapsedMilliseconds}ms, init: {localInitCounter}, body: {bodyCounter}, finally: {localFinallyCounter}");
            Console.WriteLine($"sync results: sum= {result2} elapsed time= {stopWatch2.ElapsedMilliseconds}ms");
        }

        static void ParallelInvoke()
        {
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.Invoke(parallelOptions, TestAction, () =>
            {
                Console.WriteLine($"Task #{Task.CurrentId}");
            });
        }

        static void TestAction(int i)
        {
            Console.WriteLine($"Action #{i + 1}, Thread #{Thread.CurrentThread.ManagedThreadId}");
        }

        static void TestAction()
        {
            Console.WriteLine($"TestAction Task #{Task.CurrentId}");
        }

        static object _analyseLogsLock = new object();


        static void AnalyseLogs(IEnumerable<LogItem> logs)
        {
            Parallel.ForEach(logs, log =>
            {
                var weight = log.Message.Length;
                if (weight > 100 && (int)log.Level > (int)(LogSeverity.Info))
                {
                    // less elegant code
                    lock (_analyseLogsLock)
                    {
                        ProcessLogsWeight(weight);
                    }
                }
            });
        }

        static void ZippingLogs(IEnumerable<LogItem> logs1, IEnumerable<LogItem> logs2)
        {
            var count = Math.Min(logs1.Count(), logs2.Count());
            var result = new LogItem[count];
            Parallel.ForEach(logs1,
                (elem, loopstate, index) =>
                {
                    var first = ProcessLog(elem);
                    var second = ProcessLog(logs2.ElementAt((int)index));
                    result[index] = Combine(first, second);
                });
        }

        static void ExitingFromOperations()
        {
            var matchFound = false;
            var expectedValue = 42;
            int[] source = { 1, 2, 3, 4, 5, 6, 7, 88, 42, 35, 9, 967, 123, 453, 190, 13, 12, 42 };
            var loopResult = Parallel.ForEach(source, (current, loopState) =>
            {
                if (current.Equals(expectedValue))
                {
                    matchFound = true;
                    loopState.Stop();
                }
            });
            Console.WriteLine($"{expectedValue} {(matchFound ? "was found" : "not found") } in the source");
        }

        static void BreakingLoop()
        {
            var expectedValue = 42;
            int[] source = { 1, 2, 3, 4, 5, 6, 7, 88, 42, 35, 9, 967, 123, 453, 190, 13, 12, 42 };
            var loopResult = Parallel.ForEach(source, (current, loopState) =>
            {
                if (current.Equals(expectedValue))
                {
                    loopState.Break();
                }
            });

            var matchedIndex = loopResult.LowestBreakIteration;
            var valueIndex = matchedIndex ?? -1;
        }


        static LogItem ProcessLog(LogItem log)
        {
            Thread.Sleep(50);
            return log;
        }

        static void ProcessLogsWeight(int weight)
        {
            Thread.Sleep(50);
        }

        static LogItem Combine(LogItem log1, LogItem log2)
        {
            Thread.Sleep(50);
            return log1;
        }
    }
}

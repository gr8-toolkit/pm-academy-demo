using System;
using System.Diagnostics;

namespace Task_1
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonWorker jsonWorker = new JsonWorker();
            PrimesFinder primesFinder = new PrimesFinder();
            
            SettingsWrapper settingsWrapper = jsonWorker.ReadSettings();
            if (settingsWrapper.IsSuccess == false)
            {
                jsonWorker.WriteResult(new Result(false, TimeSpan.Zero.ToString(), null, settingsWrapper.Error));
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] primes = primesFinder.FindPrimes(settingsWrapper.Settings);
            stopwatch.Stop();
                
            jsonWorker.WriteResult(new Result(true, stopwatch.Elapsed.ToString(), primes));
        }
    }
}

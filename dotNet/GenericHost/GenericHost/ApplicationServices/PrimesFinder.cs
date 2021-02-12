using System;
using System.Collections.Generic;
using GenericHost.Models.Settings;

namespace GenericHost.ApplicationServices
{
    public class PrimesFinder
    {
        public int[] FindPrimes(Settings settings)
        {
            List<int> primes = new List<int>();

            if (!settings.PrimesTo.HasValue || !settings.PrimesFrom.HasValue)
            {
                return primes.ToArray();
            }

            int start = settings.PrimesFrom.Value < 2 ? 2 : settings.PrimesFrom.Value;


            for (int i = start; i < settings.PrimesTo.Value; i++)
            {
                bool isPrime = true;
                for (int j = 2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                    primes.Add(i);
                
            }
            return primes.ToArray();
        }
    }
}
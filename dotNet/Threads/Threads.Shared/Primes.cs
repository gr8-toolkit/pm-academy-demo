using System.Collections.Generic;

namespace Threads.Shared
{
    /// <summary>
    /// Prime numbers finder.
    /// </summary>
    public class Primes
    {
        /// <summary>
        /// Searches prime numbers in range.
        /// </summary>
        /// <returns>Returns prime numbers in range.</returns>
        public List<int> FindPrimes(int from, int to)
        {
            var result = new List<int>();
            for (var i = from; i < to; i++)
            {
                if (IsPrime(i))
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private static bool IsPrime(int n)
        {
            if (n <= 1)
                return false;

            // Check from 2 to n-1
            for (var i = 2; i < n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
    }
}

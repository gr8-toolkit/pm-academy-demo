using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks.Shared
{
    public static class Primes
    {
        public static bool IsPrime(int n)
        {
            if (n == 0) return false;
            
            n = Math.Abs(n);
            
            // Check from 2 to n-1
            for (var i = 2; i < n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        public static List<int> FromRange(int from, int to)
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

        public static Task<List<int>> FromRangeAsync(int from, int to, CancellationToken token)
        {
            if (from == to)
            {
                if (token.IsCancellationRequested) 
                    return Task.FromCanceled<List<int>>(token);
                
                var shortResult = IsPrime(from)
                    ? new List<int> { from }
                    : new List<int>(0);
                
                return Task.FromResult(shortResult);
            }
            return Task.Run(() =>
            {
                var result = new List<int>();
                for (var i = from; i < to; i++)
                {
                    token.ThrowIfCancellationRequested();
                    if (IsPrime(i))
                    {
                        result.Add(i);
                    }
                }
                return result;
            }, token);
        }
    }
}

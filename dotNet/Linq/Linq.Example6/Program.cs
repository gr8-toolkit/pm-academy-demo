using System;
using System.Linq;

namespace Linq.Example6
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new[] {-2, 9, 49, -4, 0, 1};
            var result = 
                from num in numbers
                where num >= 0
                orderby num descending
                select new
                {
                    Number = num, Sqrt = Math.Sqrt(num)
                };

            var list = result.ToList();
            list.ForEach(Console.WriteLine);
            //foreach (var r in result) Console.WriteLine(r);
        }
    }
}

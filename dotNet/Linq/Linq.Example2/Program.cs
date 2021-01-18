using System;
using System.Collections.Generic;
using System.Linq;
using Linq.Data;

namespace Linq.Example2
{
    class Program
    {
        public struct ExampleStruct
        {
            public string Value { get; set; }
            
            public ExampleStruct(string value)
            {
                Value = value;
            }
        }

        private static readonly Person[] People =
        {
            new Person("Julia", 17),
            new Person("Max", 34),
            new Person("John", 19),
            new Person("Andrew", 42),
        };

        static void Main(string[] args)
        {
            //Where
            //WhereExample();
            //WhereFuncExample();

            //// ToList, ToArray, ToDictionary
            //ToListExample();
            
            //// Select
            //SelectExample();
            
            //// FirstOrDefault
            //FirstOrDefaultExample();
            //FirstStructExample();

            //// OrderBy
            //OrderByExample();
            
            //// Except
            //ExceptExample();
            
            //// Sum
            //SumExample();
            
            //// Skip and take
            //SkipTakeExample();
            //SkipTakeNumbersExample();
            
            //// Distinct
            //DistinctExample();
            
            //// Any
            AnyExample();
        }

        private static void Print<T>(IEnumerable<T> collection)
        {
            Console.WriteLine("Result :");
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }

        private static void WhereExample()
        {
            Console.WriteLine();
            Console.WriteLine("Where example");

            var adults = People.Where(p => p.Age >= 18);
            Print(adults);
        }

        private static bool IsAdult(Person p)
        {
            return p.Age >= 18;
        }

        private static void WhereFuncExample()
        {
            Console.WriteLine();
            Console.WriteLine("Where example");

            var adults = People.Where(IsAdult);
            Print(adults);
        }

        private static void FirstStructExample()
        {
            Console.WriteLine();
            Console.WriteLine("First struct example");

            var structs = Array.Empty<ExampleStruct>();
            var result = structs.FirstOrDefault();
            
            // ! ExampleStruct constructor will not be called !
            Console.WriteLine(result);
        }

        private static void ToListExample()
        {
            Console.WriteLine();
            Console.WriteLine("ToList example");

            var adults = People.Where(p => p.Age >= 18).ToList();
            adults.Add(new Person("Bob", 59));
            Print(adults);
        }

        private static void SelectExample()
        {
            Console.WriteLine();
            Console.WriteLine("Select example");

            var adultNames = People
                .Where(p => p.Age >= 18)
                .Select(p => p.Name);
            
            Print(adultNames);
        }

        private static void FirstOrDefaultExample()
        {
            Console.WriteLine();
            Console.WriteLine("FirstOrDefault example");

            var firstAdult = People.FirstOrDefault(x => x.Age < 0);
            Console.WriteLine($"First adult : {firstAdult}");
        }

        private static void OrderByExample()
        {
            Console.WriteLine();
            Console.WriteLine("OrderBy example");

            var adults = People
                //.Where(p => p.Age >= 18)
                .OrderBy(p => p.Age);
            
            Print(adults);
        }

        private static void ExceptExample()
        {
            Console.WriteLine();
            Console.WriteLine("Except example");

            var numbers1 = new[] {1, 3, 5, 5};
            var numbers2 = new[] {5, 6, 7};
            var numbers3 = new[] { 3, 4 };

            var result = numbers1.Except(numbers2).Except(numbers3);

            Print(result);
        }

        private static void SumExample()
        {
            Console.WriteLine();
            Console.WriteLine("Sum example");

            var total = People.Sum(p => p.Age);
            Console.WriteLine($"Total age: {total}");
        }

        private static void SkipTakeExample()
        {
            Console.WriteLine();
            Console.WriteLine("Skip Take example");

            var adults = People.Skip(1).Take(2);

            Print(adults);
        }

        private static void SkipTakeNumbersExample()
        {
            Console.WriteLine();
            Console.WriteLine("Skip Take numbers example");

            var numbers = new[] { 1, 2, 3, 4, 5 };
            var result = numbers.Skip(1).Take(2);

            Print(result);
        }

        private static void DistinctExample()
        {
            Console.WriteLine();
            Console.WriteLine("Distinct example");

            var numbers = new[] { 1, 3, 3, 5, 5 };

            var noDuplicates = numbers.Distinct();

            Print(noDuplicates);
        }

        private static void AnyExample()
        {
            Console.WriteLine();
            Console.WriteLine("Any example");

            var empty = Array.Empty<Person>();
            var anyKids = empty.Any();

            Console.WriteLine($"Any kids : {anyKids}");
        }
    }
}


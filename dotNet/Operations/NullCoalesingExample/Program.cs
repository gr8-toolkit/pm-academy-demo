using System;

namespace NullCoalesingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NullCoalesingExample");

            string[] example1 = new[] { "a", null, "c", "d" };
            string nullValue = example1[1];                                     // null
            string notNullValue = example1[2];                                  // "c"
            string p1 = nullValue ?? "b";
            string p2 = string.IsNullOrEmpty(notNullValue) ? "s" : "q";         // "q"
            int? p3 = nullValue?.Length;                                        // null
            //int p4 = nullValue.Length;                                        // throws NullReferenceException
            char? p5 = notNullValue?[0];                                        // 'c'
            char? p6 = nullValue?[0];                                           // null
            char p7 = nullValue[0];
        }
    }
}

using System;

namespace BranchesApp.Example5
{
    class Program
    {
        static void Main(string[] args)
        {
            int month = 3;
            if (month == 1) Console.WriteLine("Jan");
            else if (month == 2) Console.WriteLine("Feb");
            else if (month == 3) Console.WriteLine("Mar");
            else if (month == 4) Console.WriteLine("Apr");
            else if (month == 5) Console.WriteLine("May");
            else if (month == 6) Console.WriteLine("Jun");
            else if (month == 7) Console.WriteLine("Jul");
            else if (month == 8) Console.WriteLine("Aug");
            else if (month == 9) Console.WriteLine("Sep");
            else if (month == 10) Console.WriteLine("Oct");
            else if (month == 11) Console.WriteLine("Nov");
            else if (month == 12) Console.WriteLine("Dec");
            else Console.WriteLine("Invalid month");
        }
    }
}

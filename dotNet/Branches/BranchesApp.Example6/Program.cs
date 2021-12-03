using System;

namespace BranchesApp.Example6
{
    class Program
    {
        static void Main()
        {
            int month = 3;
            if ((month >= 1 && month <= 2) || month == 12)
            {
                Console.WriteLine("Winter");
            }
            else if (month >= 3 && month <= 5)
            {
                Console.WriteLine("Spring");
            }
            else if (month >= 6 && month <= 8)
            {
                Console.WriteLine("Summer");
            }
            else if (month >= 9 && month <= 11)
            {
                Console.WriteLine("Autumn");
            }
            else
            {
                Console.WriteLine("Invalid month");
            }
        }
    }
}

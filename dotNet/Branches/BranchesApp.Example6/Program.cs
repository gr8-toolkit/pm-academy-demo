using System;

namespace BranchesApp.Example6
{
    class Program
    {
        static void Main()
        {
            int month = 3;
            switch (month)
            {
                case 1:
                case 2:
                case 12: Console.WriteLine("Winter");
                    break;
                case 3:
                case 4:
                case 5:
                    Console.WriteLine("Spring");
                    break;
                case 6:
                case 7:
                case 8:
                    Console.WriteLine("Summer");
                    break;
                case 9:
                case 10:
                case 11:
                    Console.WriteLine("Autumn");
                    break;
                default:
                    Console.WriteLine("Invalid month");
                    break;
            }
        }
    }
}

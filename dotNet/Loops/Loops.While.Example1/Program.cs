using System;

namespace Loops.While.Example1
{
    class Program
    {
        static void Main()
        {
            GuessPassword();
        }

        private static void GuessPassword()
        {
            string password = "123";
            
            Console.Write("Enter the password:");
            var input = Console.ReadLine();
            
            // if input == password then skip this loop
            // otherwise try enter the password again
            while (input != password)
            {
                Console.Write("Try again : ");    
                input = Console.ReadLine();
            }
            
            Console.WriteLine("Welcome");
        }
    }
}

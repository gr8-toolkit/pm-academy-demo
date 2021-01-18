using System;

namespace Loops.Do.Example1
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
            string input;
            do
            {
                Console.Write("Enter the password:");
                input = Console.ReadLine();
            } 
            while (input != password);
            Console.WriteLine("Welcome");
        }
    }
}

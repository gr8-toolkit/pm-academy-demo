using System;

namespace BranchesApp.Example1
{

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Your age : ");
            var input = Console.ReadLine();
            var age = int.Parse(input);

            if (age >= 18 && age < 200)
            {
                Console.WriteLine("Welcome!");
            }
            else
            {
                Console.WriteLine("Invalid age");
            }

            Welcome(age);
        }

        static void Welcome(int age)
        {
            if (age < 18 || age >= 200)
            {
                throw new ArgumentOutOfRangeException(nameof(age));
            }
            Console.WriteLine("Welcome");
        }
    }
}

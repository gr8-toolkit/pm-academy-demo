using System;

namespace BranchesApp.Example2
{
    class Program
    {
        static void Main()
        {
            int age = 19;
            Welcome(age);
        }

        static void Welcome(int age)
        {
            if (age < 18 || age > 199)
            {
                throw new ArgumentOutOfRangeException(nameof(age));
            }
            Console.WriteLine("Welcome");
        }
    }
}

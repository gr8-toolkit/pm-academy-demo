using System;

namespace BranchesApp.Example2
{
    class User
    {
        public int Age { get; set; }
        public bool Verified { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var user = new User { Age = 24, Verified = true };

            if (user is null)
            {
                Console.WriteLine("User is null");
            }

            if (user is not null)
            {
                Console.WriteLine("User is not null");
            }

            if (user is User { Age: 18, Verified: true })
            {
                Console.WriteLine("User is verified and 18 years old");
            }

            if (user is User { Age: >= 18 and <= 65 })
            {
                Console.WriteLine("User can work");
            }
        }
    }
}

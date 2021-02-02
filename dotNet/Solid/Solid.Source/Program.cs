using System;
using System.Diagnostics;

namespace Solid.Source
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Good bye SOLID!");
            Test1();
        }

        private static void Test1()
        {
            var users = new UserController();
            Debug.Assert(users.Register("dima", "abc"));
            Debug.Assert(users.Register("max", "abc"));

            var token = users.Login("dima", "abc");
            Debug.Assert(token != null);
            users.UpsertUser(token, new UserDto
            {
                Name = "Dima",
                Age = 27
            });

            var user = users.GetUser(token);
            Debug.Assert(user.Name == "Dima");
        }
    }
}

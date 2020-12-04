using System;
using System.Collections.Generic;
using System.Text;

namespace ReferenceTypesExample.Custom
{
    public class HelloUser : IHello
    {
        private const string DefaultUserName = "Student";

        public int Abc { get; private set; }

        public readonly string F;

        static HelloUser()
        {

        }

        public HelloUser()
        {

        }

        public HelloUser(int abc, string f)
        {
            Abc = abc;
            F = f;
        }

        public void Hello() => Hello(DefaultUserName);

        public void Hello(string name)
        {
            Console.WriteLine($"Hello, {name}");
        }
    }
}

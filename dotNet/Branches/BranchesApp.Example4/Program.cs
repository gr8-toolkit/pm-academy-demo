using System;

namespace BranchesApp.Example4
{
    internal class Employee
    {
        public string Name { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Employee!");
            
            Employee[] employees = null;
            PrintEmployee(employees?[1]);
        }

        static void PrintEmployee(Employee employee)
        {
            //var emp = employee ?? throw new ArgumentNullException(nameof(employee));
            string name = null;

            // 1. W/O any checks
            //name = employee.Name;

            // 2. with ?. check
            //name = employee?.Name;

            // 3. with ?. and ?? check
            //name = employee?.Name ?? "<null>";

            // 4. with ??=
            employee ??= new Employee { Name = "Jack" };
            name = employee.Name;
            
            Console.WriteLine(name);
        }
    }
}

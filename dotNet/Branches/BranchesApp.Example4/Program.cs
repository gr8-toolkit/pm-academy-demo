using System;

namespace BranchesApp.Example4
{
    class Program
    {
        static void Main()
        {
            int amount = 10;
            string operation = "DEBIT";
            
            /*
            int result;
            if (operation == "CREDIT")
            {
                result = amount;
            }
            else
            {
                result = -1 * amount;
            }
            */

            int result = operation == "CREDIT" ? amount : -1 * amount;
            Console.WriteLine(result);
        }
    }
}

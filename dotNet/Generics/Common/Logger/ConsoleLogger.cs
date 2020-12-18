using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        static ConsoleLogger()
        {
            Console.WriteLine($"Initialize new console logger for [{typeof(T)}]");
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }
    }
}

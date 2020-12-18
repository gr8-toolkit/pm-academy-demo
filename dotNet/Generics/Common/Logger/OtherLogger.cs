using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public class OtherLogger<T> : ILogger<T>
    {
        static OtherLogger()
        {
            Console.WriteLine($"Initialize new other logger for [{typeof(T)}]");
        }

        public void Info(string message)
        {
            throw new NotImplementedException();
        }
    }
}

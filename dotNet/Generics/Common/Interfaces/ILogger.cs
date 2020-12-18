using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    interface ILogger<T>
    {
        void Info(string message);
    }
}

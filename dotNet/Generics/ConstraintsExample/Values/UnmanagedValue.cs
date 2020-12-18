using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class UnmanagedValue<T> where T : unmanaged
    {
        public T Data { get; set; }

        public UnmanagedValue(T data)
        {
            Data = data;
        }
    }
}

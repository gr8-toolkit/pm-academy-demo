using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class ParameterlessConstructorValue<T> where T : new()
    {
        public T Data { get; set; }

        public ParameterlessConstructorValue(T data)
        {
            Data = data;
        }
    }
}

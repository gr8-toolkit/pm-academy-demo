using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class NonNullableValue<T> where T : struct
    {
        public T Data { get; set; }

        public NonNullableValue(T data)
        {
            Data = data;
        }
    }
}

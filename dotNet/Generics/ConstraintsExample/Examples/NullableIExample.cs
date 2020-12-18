using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class NullableIExample<T> where T : IExample?
    {
        public T Data { get; set; }

        public NullableIExample(T data)
        {
            Data = data;
        }
    }
}

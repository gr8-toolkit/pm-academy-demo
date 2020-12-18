using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class NonNullableIExampleValue<T> where T : IExample
    {
        public T Data { get; set; }

        public NonNullableIExampleValue(T data)
        {
            Data = data;
        }
    }

#nullable enable

    public class NullableIExampleValue<T> where T : IExample?
    {
        public T Data { get; set; }

        public NullableIExampleValue(T data)
        {
            Data = data;
        }
    }

}

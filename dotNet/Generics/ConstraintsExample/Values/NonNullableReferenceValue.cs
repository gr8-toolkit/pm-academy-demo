using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ConstraintsExample
{
    public class NonNullableReferenceValue<T> where T : class
    {
        public T Data { get; private set; }

        public NonNullableReferenceValue(T data)
        {
            Data = data;
        }
    }
}

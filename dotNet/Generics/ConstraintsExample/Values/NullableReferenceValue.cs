using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ConstraintsExample
{
    public class NullableReferenceValue<T> where T : class?
    {
        public T? Data { get; set; }

        public NullableReferenceValue(T? data)
        {
            Data = data;
        }
    }
}

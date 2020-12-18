using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ConstraintsExample
{
    public class NotNullValue<T> where T : notnull
    {
        public T Data { get; set; }

        public NotNullValue(T data)
        {
            Data = data;
        }
    }
}

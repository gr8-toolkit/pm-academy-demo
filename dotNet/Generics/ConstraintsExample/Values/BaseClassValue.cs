using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class BaseClassValue<T> where T : BaseClass
    {
        public T Data { get; set; }

        public BaseClassValue(T data)
        {
            Data = data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstraintsExample
{
    public class DerivedTypeInValue<T, U, V> 
        where T : IAppender<V>
        where U: class
    {
        public T Appender { get; set; }
        public V DerivedData { get; set; }
        public U Data { get; private set; }

        public DerivedTypeInValue(U data)
        {
            Data = data;
        }

        public void AppendDerivedData()
        {
            Appender.Append(DerivedData);
        }
    }
}

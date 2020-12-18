using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialExample
{
    public class GenericPropertyExample
    {
        public GenericProperty<int> GenPropInt32 { get; set; }
        public GenericProperty<string> GenPropString { get; set; }
    }

    public class GenericPropertyExampleV2<T1, T2>
    {
        public GenericProperty<T1> GenPropM1 { get; set; }
        public GenericProperty<T2> GenPropM2 { get; set; }
    }

    public class GenericProperty<T>
    {
        private T _value;

        public T Value
        {
            get
            {
                // some getter logic
                return _value;
            }
            set
            {
                // some setter logic
                _value = value;
            }
        }

        public static implicit operator T(GenericProperty<T> value)
        {
            return value.Value;
        }

        public static implicit operator GenericProperty<T>(T value)
        {
            return new GenericProperty<T> { Value = value };
        }
    }
}

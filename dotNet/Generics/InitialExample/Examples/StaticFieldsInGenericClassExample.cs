using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialExample
{

    public class StaticFieldsInGenericClass<T>
    {
        // static field is not part of a specific instance
        // it is shared amongst all instances of a closed type (T)
        public static int StaticInt32Value;
        public static List<T> StaticList;

        static StaticFieldsInGenericClass()
        {
            StaticInt32Value = 111;
            StaticList = new List<T>();
        }
    }
}

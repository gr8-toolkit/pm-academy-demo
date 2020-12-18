using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialExample
{
    public class GenericConstructorExample
    {
        public GenericConstructorExample()
        {
            var inst1 = Foo.CreateInstance<int>(123);
            var inst2 = Foo.CreateInstance<string>("123");
        }
    }
    public static class Foo
    {
        public static Foo<T> CreateInstance<T>(T instance)
        {
            return new Foo<T>(instance);
        }
    }

    public class Foo<T>
    {
        private readonly T _innerData;

        public Foo(T instance)
        {
            _innerData = instance;
        }
    }
}

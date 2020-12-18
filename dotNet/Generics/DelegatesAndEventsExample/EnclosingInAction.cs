using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesAndEventsExample
{
    public static class EnclosingInAction
    {
        public delegate void Tralivali();

        public static void Foo()
        {
            var outerValiable = 123;                            // not captured
            var capturedVariable = "123";                       // captured

            if (DateTime.Now.Minute % 2 == 0)
            {
                var localVariable = DateTime.Now.Minute;        // local variable in Foo
                Console.WriteLine($"M {localVariable}");
            }

            Tralivali tilitili = delegate ()
            {
                var localVariable = "456";                       // local variable in anonymous method
                Console.WriteLine($"{capturedVariable} {localVariable}");
            };

            tilitili?.Invoke();
        }
    }
}

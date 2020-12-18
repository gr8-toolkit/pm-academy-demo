using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample.Examples
{
    public static class SomeFactoryExample
    {
        delegate ClassA SomeFactory();

        public static void Foo()
        {
            SomeFactory factory = GenerateSample;       // result covariance
            var result = factory.Invoke();              // ClassA
        }

        private static ClassB GenerateSample() => new ClassB();

        internal class ClassA { }

        internal class ClassB : ClassA { }
    }

}

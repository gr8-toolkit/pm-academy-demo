using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample.Examples
{
    public static class CovarianceExample
    {
        delegate DerivedExample DerivedExampleFactory();
        delegate BaseExample BaseExampleFactory();
        delegate T Factory1<out T>();
        delegate T Factory2<T>();

        public static void Foo()
        {
            Console.WriteLine("Covariance example");

            var exampleM1 = new BaseExample();
            IExample example = exampleM1;
            CovarianceInterfaces();
            CovarianceDelegates();
        }

        private static void CovarianceInterfaces()
        {
            List<BaseExample> listOfBaseExamples = new List<BaseExample>();
            List<DerivedExample> listOfDerivedExamples = new List<DerivedExample>();

            // IEnumerable<out T> ->> covariance
            IEnumerable<IExample> collectionOfExamplesFromBase = listOfBaseExamples;
            IEnumerable<BaseExample> collectionOfBaseExamplesFromDerived = listOfDerivedExamples;

            // IList<T> ->> variance
            //IList<IExample> listOfExamplesFromBase2 = listOfBaseExamples;
            //IList<BaseExample> listOfBaseExamplesFromDerived2 = listOfDerivedExamples;
        }

        private static void CovarianceDelegates()
        {
            Factory1<DerivedExample> derivedTypeFactoryM1E1 = MakeDerivedInstance;
            // covariance enables this assignment
            Factory1<BaseExample> baseTypeFactoryM1E1 = derivedTypeFactoryM1E1;
            Factory1<BaseExample> baseTypeFactoryM1E2 = MakeDerivedInstance;
            BaseExampleFactory baseExampleFactory = MakeDerivedInstance;

            Factory2<DerivedExample> derivedTypeFactoryM2E1 = MakeDerivedInstance;
            // error
            //Factory2<BaseExample> baseTypeFactoryM2E1  = derivedTypeFactoryM2E1;
        }

        private static DerivedExample MakeDerivedInstance() => new DerivedExample();

        private static BaseExample MakeBaseInstance() => new BaseExample();

        internal class BaseExample : IExample { }

        internal class DerivedExample : BaseExample { }

        internal interface IExample { }
    }
}

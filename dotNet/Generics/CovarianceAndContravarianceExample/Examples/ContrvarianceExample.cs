using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceAndContravarianceExample.Examples
{
    public static class ContrvarianceExample
    {
        public delegate void Action1<in T>(T p);
        public delegate void Action2<T>(T p);

        public static void Foo()
        {
            Console.WriteLine("Contrvariance example");

            ContrvarianceInterfaces();
            ContrvarianceDelegates();
        }

        private static void ContrvarianceInterfaces()
        {
            IWrapperExample1<BaseExample> baseClassInst = new ExampleWrapper1<BaseExample>();
            // contrvariance
            IWrapperExample1<DerivedExample> derivedClassInst = baseClassInst;

            IWrapperExample2<BaseExample> baseClassInst2 = new ExampleWrapper2<BaseExample>();
            // error
            //IWrapperExample2<DerivedExample> derivedClassInst2 = baseClassInst2;
        }

        private static void ContrvarianceDelegates()
        {
            Action1<BaseExample> baseClassAction = ActionOnBaseClass;
            // contrvariance
            Action1<DerivedExample> derivedClassAction = baseClassAction;

            Action2<BaseExample> baseClassAction2 = ActionOnBaseClass;
            // error
            //Action2<DerivedExample> derivedClassAction2 = baseClassAction2;
        }

        static void ActionOnDerivedClass(DerivedExample p) => Console.WriteLine("");

        static void ActionOnBaseClass(BaseExample p) => Console.WriteLine("");

        static DerivedExample MakeDerivedInst() => new DerivedExample();

        static BaseExample MakeBaseInst() => new BaseExample();

        internal class BaseExample : IExample { }

        internal class DerivedExample : BaseExample { }

        internal interface IExample { }

        internal interface IWrapperExample1<in T> { }

        internal interface IWrapperExample2<T> { }

        internal class ExampleWrapper1<T> : IWrapperExample1<T> { }

        internal class ExampleWrapper2<T> : IWrapperExample2<T> { }
    }

}

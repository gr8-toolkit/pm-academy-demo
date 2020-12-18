using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesAndEventsExample
{
    public static class BuildinDelegates
    {
        delegate int BinaryOperation(int a, int b);
        delegate T BinaryOperationU<T>(T a, T b);
        delegate bool IsPositivePredicate(int a);
        delegate void SimpleOperation(int a, int b);
        delegate void SimpleOperationU<T>(T a, T b);

        public static void Foo()
        {
            Func<int, int, int> operationM11;
            BinaryOperation operationM12;
            BinaryOperationU<int> operationM13;
            Func<int, int, int> operationM14;
            Func<int, int, int> operationM15;

            Action<int, int> operationM21;
            SimpleOperation operationM22;
            SimpleOperationU<int> operationM23;
            Action<int, int> operationM24;
            Action<int, int> operationM25;

            Predicate<int> operationM31;
            IsPositivePredicate operationM32;
            Predicate<int> operationM33;
            Predicate<int> operationM34;

            operationM11 = Add;
            operationM12 = Add;
            operationM13 = Add;
            operationM14 = (int a, int b) => a + b;
            operationM15 = delegate (int a, int b) { return a + b; };

            operationM21 = WriteSumInConsole;
            operationM21 += WriteDiffInConsole;
            operationM22 = WriteSumInConsole;
            operationM23 = WriteDiffInConsole;
            operationM24 = (int a, int b) => Console.WriteLine(a * b);
            operationM25 = delegate (int a, int b) { Console.WriteLine(a * b); };

            operationM31 = IsPositive;
            operationM32 = IsPositive;
            operationM33 = (int a) => a > 0;
            operationM34 = delegate (int a) { return a > 0; };

            operationM21(124, 541);                             // Console: 665 \r\n -417

            var invocationListM21 = operationM21.GetInvocationList();

            foreach (var d in invocationListM21)
            {
                d.DynamicInvoke(11, 2);                         // Console: 13 \r\n 9
            }

            var sumInConsole = (Action<int, int>)invocationListM21[0];
            var diffInConsole = (Action<int, int>)invocationListM21[1];

            sumInConsole(1, 2);                                 // Console: 3
            diffInConsole(1, 2);                                // Console: -1
        }

        static int Add(int a, int b) => a + b;
        static int Sub(int a, int b) => a - b;

        static bool IsPositive(int a) => a > 0;
        static bool IsNegative(int a) => !IsPositive(a);

        static void WriteSumInConsole(int a, int b)
        {
            Console.WriteLine(a + b);
        }
        static void WriteDiffInConsole(int a, int b)
        {
            Console.WriteLine(a - b);
        }
    }
}

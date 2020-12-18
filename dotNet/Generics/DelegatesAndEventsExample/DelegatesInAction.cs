using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesAndEventsExample
{
    public class DelegatesInAction
    {
        public static void Foo()
        {
            BinaryOperation add = Add;
            BinaryOperationG<int> add2 = Add;

            BinaryOperation sub = Sub;
            int addRes = add(0, 5);                     // 5
            int subRes = sub.Invoke(1, 4);              // -3

            Action<int> a1 = WriteHello;
            a1 += WriteHowYouDo;

            Predicate<int> p1 = PositiveNumber;
            Func<int, string> f1 = NumberStr;
            Func<string> f2 = DateStr;
            Func<int, int, int> f3 = Add;

            a1?.Invoke(1);                              // void, write in console
            bool p1r = p1(123);                         // true
            string f1r = f1(6);                         // "6"
            string f2r = f2();                          // 2020 12 16


            Func<int, string> f4 = delegate (int a)
            {
                return a.ToString();
            };

            Func<int, string> f5 = (a) => a.ToString();

            try
            {

                Func<int, int, int> f44 = null;
                //var d1 = f44(4, 5);
                var d2 = f44?.Invoke(4, 5);
            }
            catch (NullReferenceException e)
            {
                var message = e.Message;
            }
        }

        delegate int BinaryOperation(int a, int b);

        delegate T BinaryOperationG<T>(T a, T b) where T: struct;

        static int Add(int a, int b) => a + b;

        static int Sub(int a, int b) => a - b;

        static void WriteHello(int a) => Console.WriteLine("hello");

        static void WriteHowYouDo(int a) => Console.WriteLine("how do you do?");

        static bool PositiveNumber(int a) => a > 0;

        static string NumberStr(int a) => a.ToString();

        static string DateStr() => DateTime.Now.ToString("yyyy MM dd");
    }
}

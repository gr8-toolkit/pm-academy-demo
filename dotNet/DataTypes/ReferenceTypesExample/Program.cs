using Common;
using ReferenceTypesExample.Custom;
using System;

namespace ReferenceTypesExample
{
    class Program
    {
        public object ObjExample1Value => 12243;
        public object ObjExample2Value => 22 / 7;
        public object ObjExample3Value => "3.14";
        public object ObjExample4Value => null;
        public string StringExampleValue => "text";

        public dynamic DynExample1Value => 12243;
        public dynamic DynExample2Value => "dyn";

        delegate void DelegateExample();
        delegate void DelegateExampleWithStr(string name);

        public int[] Int32ArrayExample1 => new int[] { 1, 4, 5 };           // 1,4,5
        public int[] Int32ArrayExample2 => new int[3];                      // 0, 0, 0

        static void Main(string[] args)
        {
            Console.WriteLine("ReferenceTypesExample");
            ClassInAction();
            DelegeteInAction();
            CompareStrings();
            ParseStrings();
        }

        #region reference type in action

        static void ClassInAction()
        {
            var rfEx1 = new ClassExample() { Foo = "initial-string-literal", Bar = 123 };

            var rfEx2 = rfEx1;                          // rfEx2.Foo="initial-string-literal", rfEx2.Bar=123

            rfEx1.Foo = "1";                            // rfEx1.Foo="1", rfEx2.Foo="1"
            rfEx1.Bar = 1;                              // rfEx1.Bar=1, rfEx2.Bar=1

            ChangeExampleValues(rfEx1);                 // rfEx1.Foo="2", rfEx2.Foo="2"
                                                        // rfEx1.Bar=2, rfEx2.Bar=2

            ChangeExampleReference(ref rfEx1);          // rfEx1.Foo="3", rfEx2.Foo="2"
                                                        // rfEx1.Bar=3, rfEx2.Bar=2

            rfEx1.Foo = "4";                            // rfEx1.Foo="4", rfEx2.Foo="2"
            rfEx1.Bar = 4;                              // rfEx1.Bar=4, rfEx2.Bar=2
        }

        static void ChangeExampleValues(ClassExample example)
        {
            example.Bar = 2;
            example.Foo = "2";

            example = new ClassExample
            {
                Foo = "secondary-string-literal",
                Bar = 222
            };
        }

        static void ChangeExampleReference(ref ClassExample example)
        {
            example = new ClassExample
            {
                Foo = "3",
                Bar = 3
            };
        }

        #endregion

        #region delegate in action

        static void DelegeteInAction()
        {
            DelegateExample delExE1 = Hello;            // reference 
            DelegateExampleWithStr delExE2;             // null
            delExE2 = null;                             // null
            delExE1.Invoke();
            delExE2?.Invoke("Ping pong");               // nothing

            delExE2 = HelloUser;                        // reference
            delExE2?.Invoke("Batman");                  // write in console "Hello, Batman"
        }

        static void Hello()
        {
            Console.WriteLine("Hello");
        }

        static void HelloUser(string userName)
        {
            Console.WriteLine($"Hello, {userName}");
        }

        #endregion

        #region strings in action

        static void CompareStrings()
        {
            string strE1 = "hell0";             // "hell0"
            string strE2 = strE1;               // "hell0"
            bool eq1 = strE1 == strE2;          // true
            strE1 = "hello";
            bool eq2 = strE1 == strE2;          // false
            strE1 += " ";                       // strE1="hello ", strE2="hello"
            strE1 += "world [1]";               // "hello world [1]"
        }

        static void ParseStrings()
        {
            try
            {
                string stringE1 = "112356";                     // "112356"
                int intParsedE1 = int.Parse(stringE1);          // 112356
                string stringE2 = "string-literal";             // "string-literal"
                //int intParsedE2 = int.Parse(stringE2);        // FormatException: Input string was not in a correct format.
                string stringE3 = "2020-04-06";                 // 2020-04-06
                DateTime dateTimeE1 = DateTime.Parse(stringE3); // {4/6/2020 12:00:00 AM}

                string stringE4 = "Max";
                object enumInt32ExampleE1 = Enum.Parse(typeof(EnumInt32Example), stringE4);                                 // EnumInt32Example.Max
                EnumInt32Example enumInt32ExampleE2 = (EnumInt32Example)Enum.Parse(typeof(EnumInt32Example), stringE4);     // EnumInt32Example.Max
                int enumInt32ExampleE3 = (int)Enum.Parse(typeof(EnumInt32Example), stringE4);                               // 2147483647

                object enumUInt64ExampleE1 = Enum.Parse(typeof(EnumUInt64Example), stringE4);                               // EnumUInt64Example.Max
                EnumUInt64Example enumUInt64ExampleE2 = (EnumUInt64Example)Enum.Parse(typeof(EnumUInt64Example), stringE4); // EnumUInt64Example.Max
                ulong enumUInt64ExampleE3 = (ulong)Enum.Parse(typeof(EnumUInt64Example), stringE4);                         // 18446744073709551615
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}

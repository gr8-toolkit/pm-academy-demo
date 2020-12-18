using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace InitialExample
{
    // DRY: don't repeat yourself
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generics initial");
            CollectoinsBeforeGenerics();
            CollectionsComparison();
            GenericListInAction();
            GenericCollectionInit();
            GenericMethodsInAction();
            NullableValueExample();
            GenericsWithStatic();
            Console.ReadLine();
        }

        private static void CollectoinsBeforeGenerics()
        {
            var int32ArrayM1 = new int[] { 1, 2, 3 };               // array
            var strArrayM1 = new string[] { "1", "2", "3" };        // array
            var strArrayM2 = new string[3];                         // array
            var hasTableM1 = new Hashtable();                       // object based key-value collection
            var arrayListM1 = new ArrayList();                      // object based collection
            var stringsM1 = new StringCollection();                 // special strings collection

            strArrayM2[0] = "1";
            strArrayM2[1] = "2";
            strArrayM2[2] = "3";
            // error: IndexOutOfRangeException
            //strArrayM2[3] = "4"; 

            arrayListM1.Add("v1");
            arrayListM1.Add(123);

            hasTableM1.Add("k1", "v1");
            hasTableM1.Add("k2", 123);

            stringsM1.Add("v1");
            stringsM1.Add(string.Empty);
            stringsM1.Add(null);

            // error: cannot convert from 'int' to 'string?'
            //stringsM1.Add(123);

            var int32ValueM1 = int32ArrayM1[0];                      // int32
            var strValueM1 = strArrayM1[0];                          // string
            var strValueM2 = strArrayM2[0];                          // string

            var hashValueM1 = hasTableM1["k1"];                     // object "v1"
            var hashValueM2 = hasTableM1["k2"];                     // object 123
            var hashValueM3 = hasTableM1["k3"];                     // object null

            var listValueM1 = arrayListM1[0];                       // object "v1"
            var listValueM2 = arrayListM1[1];                       // object 123
            // error: ArgumentOutOfRangeException
            //var listValueM3 = arrayListM1[2];

            var stringsValueM1 = stringsM1[0];                      // string
            var stringsValueM2 = stringsM1[1];                      // string
            var stringsValueM3 = stringsM1[2];                      // string
            // error: ArgumentOutOfRangeException
            //var stringsValueM4 = stringsM1[3];
        }

        private static void CollectionsComparison()
        {
            var int32List = new List<int>();
            var strList = new List<string>();
            var dictStrInt = new Dictionary<int, string>();
            var objArray = new ArrayList();

            int32List.Add(123);

            // error: cannot convert from 'string' to 'int'
            // int32List.Add("123"); 

            strList.Add("123");

            dictStrInt.Add(1, "first");
            dictStrInt.Add(2, "second");
            // error: cannot convert from 'string' to 'int'
            // error: cannot convert from 'int' to 'string'
            //dictStrInt.Add("third", 3);

            objArray.Add(123);                          // box int32
            objArray.Add(123.456);                      // box double
            objArray.Add(123.456m);                     // box decimal
            objArray.Add("789");

            var elem1 = objArray[0];                    // object
            var elem2 = objArray[1];                    // object
            var elem3 = objArray[2];                    // object
            var elem4 = objArray[3];                    // object

            // Сompile error: Operator '+' cannot be applied to operands of type 'object' and 'object'
            //var sum1 = elem1 + elem2;
            var sum2 = (int)elem1 + (double)elem2;      // double  246.45600000000002
            var sum3 = (int)elem1 + (decimal)elem3;     // decimal
            var eq1 = sum2 == 246.456;                  // false
            var eq2 = sum3 == 246.456m;                 // true
        }

        private static void GenericListInAction()
        {
            var int32GenList = new GenericList<int>();
            var strGetList = new GenericList<string>();
            var objGetList = new GenericList<object>();
            int32GenList.Add(123);
            strGetList.Add("123");
            objGetList.Add(123);
            objGetList.Add("123");
        }

        private static void GenericCollectionInit()
        {
            var int32ListM1 = new List<int>
            {
                1,2,3
            };
            // 1,2,3
            var int32ListM2 = new List<int>(int32ListM1)
            {
                4,5,6
            };
            // 1,2,3,4,5,6
            var int32ListM3 = new List<int>(Int32EnumerableExample());
            // 1,2,3
            var int32ListM4 = new List<int>(new int[] { 1, 2, 3 });
            // 1,2,3
            var int32ListM5 = new List<int>(3);
            // empty

            var puma = new Car("Puma", Color.Blue);
            var carListM1 = new List<Car>
            {
                new Car("F150", Color.Black),
                new Car("Mustang", Color.Red),
                puma
            };
        }

        private static void GenericMethodsInAction()
        {
            var f150 = new Car("F150", Color.Black);
            var mustang = new Car("Mustang", Color.Red);
            Swap(ref f150, ref mustang);

            int x = 1, y = 2;
            Swap(ref x, ref y);
            // Error
            //Swap(ref x, ref mustang);
        }

        private static void Swap<T>(ref T x, ref T y)
        {
            var temp = x;
            x = y;
            y = temp;
        }

        private static IEnumerable<int> Int32EnumerableExample()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        private static void NullableValueExample()
        {
            int? nullableInteger1 = 123;
            int? nullableInteger2 = null;
            Nullable<int> nullableInteger3 = 123;
            Nullable<int> nullableInteger4 = null;

            var hasValue1 = nullableInteger1.HasValue;              // true
            var hasValue2 = nullableInteger2.HasValue;              // false
            var hasValue3 = nullableInteger3.HasValue;              // true
            var hasValue4 = nullableInteger4.HasValue;              // false

            var eq1 = nullableInteger1 == 123;                      // true
            var eq2 = nullableInteger2 == null;                     // true
            var eq3 = nullableInteger3 == null;                     // false
            var eq4 = nullableInteger4 == null;                     // true

            nullableInteger1++;                                     // 124
            nullableInteger2++;                                     // null

            // error: The type 'string' must be a non-nullable value type in order to use it as parameter 'T' in the generic type or method 
            //Nullable<string> nullableString = null;

            var nullableInteger5 = nullableInteger1 ?? 1;           // 124
            var nullableInteger6 = nullableInteger2 ?? 1;           // 1

            var list1 = new List<int?>
            {
                123,
                null
            };
        }

        private static void GenericsWithStatic()
        {
            var inst1 = new StaticFieldsInGenericClass<int>();
            var inst2 = new StaticFieldsInGenericClass<int>();
            var inst3 = new StaticFieldsInGenericClass<string>();
            var inst4 = new StaticFieldsInGenericClass<string>();

            var stFooM1 = StaticFieldsInGenericClass<int>.StaticInt32Value;              // 111
            var stFooM2 = StaticFieldsInGenericClass<string>.StaticInt32Value;           // 111

            StaticFieldsInGenericClass<int>.StaticInt32Value = 123;                      // 123
            StaticFieldsInGenericClass<string>.StaticInt32Value = 456;                   // 456

            var stFooM3 = StaticFieldsInGenericClass<int>.StaticInt32Value;
            var stFooM4 = StaticFieldsInGenericClass<string>.StaticInt32Value;

            StaticFieldsInGenericClass<int>.StaticList.Add(123);
            var stListCount1 = StaticFieldsInGenericClass<int>.StaticList.Count;         // 1
            var stListCount2 = StaticFieldsInGenericClass<string>.StaticList.Count;      // 0
        }
    }
}
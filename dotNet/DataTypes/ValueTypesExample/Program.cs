using Common;
using System;
using System.Runtime.InteropServices;

namespace ValueTypesExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ValueTypesExample");

            try
            {
                //StructExampleValue.Param1 = 12;                   // Erorr: read only field can not be assigned

                int int32E1M1 = 123;                                // Int32 = 123
                /* неявне перетворення / implict casting */
                double doubleM1 = int32E1M1;                        // Double = 123


                /* автоматичне перетворення типу */
                long int64M1 = int32E1M1;                           // Int64 = 123
                float floatM1 = int64M1;                            // Single = 123

                char charM1 = '6';                                  // Char = '6'
                int int32E1M2 = charM1;                             // Int32 = 54

                var uint64E1 = ulong.MaxValue - 123;                // UInt64 = 18446744073709551492

                /*
                 * контекст НЕ перевіряється
                 * отриманий результат = відрізання старших бітів
                 * по замовчуванню перевірка вимкнена
                 */
                var int32OverflowE1M1 = (int)uint64E1;              // Int32 overflow = -124

             

               


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}

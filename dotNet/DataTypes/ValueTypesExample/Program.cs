using Common;
using System;
using ValueTypesExample.Custom;

namespace ValueTypesExample
{
    class Program
    {

        #region Build in value types.

        public sbyte SbyteMaxValue => sbyte.MaxValue;               // 127
        public sbyte SbyteMinValue => sbyte.MinValue;               // -128
        public sbyte SbyteDefValue => default;                      // 0

        public byte ByteMaxValue => byte.MaxValue;                  // 255
        public byte ByteMinValue => byte.MinValue;                  // 0
        public byte ByteDefValue => default;                        // 0

        public short ShortMaxValue => short.MaxValue;               // 32767
        public short ShorMinValue => short.MinValue;                // -32768
        public short ShorDefValue => default;                       // 0

        public ushort UshortMaxValue => ushort.MaxValue;            // 65535
        public ushort UshortMinValue => ushort.MinValue;            // 0
        public ushort UshortDefValue => default;                    // 0

        public int IntMaxValue => int.MaxValue;                     // 2147483647
        public int IntMinValue => int.MinValue;                     // -2147483648
        public int IntDefValue => default;                          // 0

        public uint UintMaxValue => uint.MaxValue;                  // 4294967295
        public uint UintMinValue => uint.MinValue;                  // 0
        public uint UintDefValue => default;                        // 0

        public long LongMaxValue => long.MaxValue;                  // 9223372036854775807
        public long LongMinValue => long.MinValue;                  // -9223372036854775808
        public long LongDefValue => default;                        // 0

        public ulong UlongMaxValue => ulong.MaxValue;               // 18446744073709551615
        public ulong UlongMinValue => ulong.MinValue;               // 0
        public ulong UlongDefValue => default;                      // 0

        public float FloatMaxValue => float.MaxValue;               // 3.40282347E+38F
        public float FloatMinValue => float.MinValue;               // -3.40282347E+38F
        public float FloatDefValue => default;                      // 0

        public double DoubleMaxValue => double.MaxValue;            // 1.7976931348623157E+308
        public double DoubleMinValue => double.MinValue;            // -1.7976931348623157E+308
        public double DoubleDefValue => default;                    // 0

        public decimal DecimalMaxValue => decimal.MaxValue;         // 79228162514264337593543950335M
        public decimal DecimalMinValue => decimal.MinValue;         // -79228162514264337593543950335M
        public decimal DecimalDoubleDefValue => default;            // 0

        public bool BoolTrueValue => true;                          // true
        public bool BoolFalseValue => false;                        // false
        public bool BoolDefValue => default;                        // false

        public char CharMaxValue => char.MaxValue;                  // '\uffff'
        public char CharMinValue => char.MinValue;                  // '\0'
        public char CharDefValue => default;                        // '\0'

        public DateTime DateTimeMinValue => DateTime.MinValue;      // 1/1/0001 12:00:00 AM
        public DateTime DateTimeMaxValue => DateTime.MaxValue;      // 12/31/9999 11:59:59 PM

        #endregion

        #region Custom value types.
        public StructExample StructExampleValue => new StructExample(1112, 2, false);
        public EnumInt32Example EnumExampleValue => EnumInt32Example.Unknown;
        public (double, int) TupleExamlpeValue => (4.5, 3);
        #endregion

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

                /*
                 * перевірка конексту згенерує виключення
                 * OverflowException: Arithmetic operation resulted in an overflow.
                 */
                //var int32OverflowE1M3 = checked((int)uint64E1);

                /* використовуємо конвертацію, з використанням методів із IConvertible */

                // OverflowException: Value was either too large or too small for an Int32.
                //int int32OverflowE1M2 = Convert.ToInt32(uint64E1);

                var doubleE2 = -2.224;
                var int32E2M1 = Convert.ChangeType(doubleE2, typeof(int));              // object
                var int32E2M2 = (int)Convert.ChangeType(doubleE2, typeof(int));         // System.Int32 = -2
                var int32E2M3 = Convert.ToInt32(doubleE2);                              // System.Int32 = -2
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        static void ByteImplicitConvertion()
        {
            byte byteE1 = 112;
            short shortE1 = byteE1;
            ushort ushortE1 = byteE1;
            int intE1 = byteE1;
            uint uintE1 = byteE1;
            long longE1 = byteE1;
            ulong ulongE1 = byteE1;
            float floatE1 = byteE1;
            double doubleE1 = byteE1;
            decimal decimalE1 = byteE1;
        }

        static void ShortImplicitConvertion()
        {
            short shortE1 = 11211;
            int intE1 = shortE1;
            long longE1 = shortE1;
            float floatE1 = shortE1;
            double doubleE1 = shortE1;
            decimal decimalE1 = shortE1;
        }
        static void IntImplicitConvertion()
        {
            int intE1 = 1221231331;
            long longE1 = intE1;
            long floatE1 = intE1;
            long doubleE1 = intE1;
            long decimalE1 = intE1;
        }

        static void LongImplicitConvertion()
        {
            long longE1 = 12212311122331;
            float floatE1 = longE1;
            double doubleE1 = longE1;
            decimal decimalE1 = longE1;
        }

        static void CharImplicitConvertion()
        {
            char charE1 = 'O';
            ushort ushortE1 = charE1;
            int intE1 = charE1;
            uint uintE1 = charE1;
            long longE1 = charE1;
            ulong ulongE1 = charE1;
            float floatE1 = charE1;
            double doubleE1 = charE1;
            decimal decimalE1 = charE1;
        }

        static void FloatImplicitConvertion()
        {
            float floatE1 = 11212.2f;
            double doubleE1 = floatE1;
        }

        static void BoolImplicitConvertion()
        {
            Console.WriteLine("No implicit conversion for Bool");
        }

        static void DoubleImplicitConvertion()
        {
            Console.WriteLine("No implicit conversion for Double");
        }

        static void DecimalImplicitConvertion()
        {
            Console.WriteLine("No implicit conversion for Decimal");
        }

        static void ByteExplicitConvertion()
        {
            byte byteE1 = 128;
            sbyte sbyteE1 = (sbyte)byteE1;
            char charE1 = (char)byteE1;
        }


        static void ShortExplicitConvertion()
        {
            short shortE1 = 21221;
            sbyte sbyteE1 = (sbyte)shortE1;
            byte byteE1 = (byte)shortE1;
            ushort ushortE1 = (ushort)shortE1;
            uint uintE1 = (uint)shortE1;
            ulong ulongE1 = (ulong)shortE1;
            char charE1 = (char)shortE1;
        }

        static void IntExplicitConvertion()
        {
            int intE1 = 123421212;
            sbyte sbyteE1 = (sbyte)intE1;
            byte byteE1 = (byte)intE1;
            short shortE1 = (short)intE1;
            ushort ushortE1 = (ushort)intE1;
            uint uintE1 = (uint)intE1;
            ulong ulongE1 = (ulong)intE1;
            char charE1 = (char)intE1;
        }

        static void LongExplicitConvertion()
        {
            long longE1 = 1234211125761212;
            sbyte sbyteE1 = (sbyte)longE1;
            byte byteE1 = (byte)longE1;
            short shortE1 = (short)longE1;
            ushort ushortE1 = (ushort)longE1;
            int intE1 = (int)longE1;
            uint uintE1 = (uint)longE1;
            ulong ulongE1 = (ulong)longE1;
            char charE1 = (char)longE1;
        }

        static void CharExplicitConvertion()
        {
            char charE1 = 'Q';
            sbyte sbyteE1 = (sbyte)charE1;
            byte byteE1 = (byte)charE1;
            short shortE1 = (short)charE1;
        }

        static void FloatExplicitConvertion()
        {
            float floatE1 = 1113.34f;
            sbyte sbyteE1 = (sbyte)floatE1;
            byte byteE1 = (byte)floatE1;
            short shortE1 = (short)floatE1;
            ushort ushortE1 = (ushort)floatE1;
            int intE1 = (int)floatE1;
            uint uintE1 = (uint)floatE1;
            long longE1 = (long)floatE1;
            ulong ulongE1 = (ulong)floatE1;
            char charE1 = (char)floatE1;
            decimal decimalE1 = (decimal)floatE1;
        }

        static void DouleExplicitConvertion()
        {
            double doubleE1 = 133113.34944f;
            sbyte sbyteE1 = (sbyte)doubleE1;
            byte byteE1 = (byte)doubleE1;
            short shortE1 = (short)doubleE1;
            ushort ushortE1 = (ushort)doubleE1;
            int intE1 = (int)doubleE1;
            uint uintE1 = (uint)doubleE1;
            long longE1 = (long)doubleE1;
            ulong ulongE1 = (ulong)doubleE1;
            char charE1 = (char)doubleE1;
            float floatE1 = (float)doubleE1;
            decimal decimalE1 = (decimal)doubleE1;
        }

        static void DecimalExplicitConvertion()
        {
            double decimalE1 = 133112213.32234944f;
            sbyte sbyteE1 = (sbyte)decimalE1;
            byte byteE1 = (byte)decimalE1;
            short shortE1 = (short)decimalE1;
            ushort ushortE1 = (ushort)decimalE1;
            int intE1 = (int)decimalE1;
            uint uintE1 = (uint)decimalE1;
            long longE1 = (long)decimalE1;
            ulong ulongE1 = (ulong)decimalE1;
            char charE1 = (char)decimalE1;
            float floatE1 = (float)decimalE1;
            double doubleE1 = (double)decimalE1;
        }


    }
}

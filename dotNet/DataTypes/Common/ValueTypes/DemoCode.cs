using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ValueTypes
{
    internal class DemoCode
    {
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
        public int IntHex16Value => 0xFA;                           // 250
        public int IntBin2Value => 0b1111101;                       // 125

        public nint IntPtrMinValue => nint.MinValue;                // int.MaxValue for 32 bit system, long.MaxValue for 64 bit system
        public nint IntPtrMaxValue => nint.MaxValue;                // int.MinValue for 32 bit system, long.MinValue1 for 64 bit system
        public nint IntPtrDefValue => default;                      // 0

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
        public float FloatSuffixValue => 3.1428F;                   // 3.1428, F suffix

        public double DoubleMaxValue => double.MaxValue;            // 1.7976931348623157E+308
        public double DoubleMinValue => double.MinValue;            // -1.7976931348623157E+308
        public double DoubleDefValue => default;                    // 0

        public decimal DecimalMaxValue => decimal.MaxValue;         // 79228162514264337593543950335M
        public decimal DecimalMinValue => decimal.MinValue;         // -79228162514264337593543950335M
        public decimal DecimalDoubleDefValue => default;            // 0
        public decimal DecimalSuffuxValue => 10010.442M;            // 10010.442M, M suffix

        public bool BoolTrueValue => true;                          // true literal
        public bool BoolFalseValue => false;                        // false literal
        public bool BoolDefValue => default;                        // false

        public char CharMaxValue => char.MaxValue;                  // '\uffff'
        public char CharMinValue => char.MinValue;                  // '\0'
        public char CharDefValue => default;                        // '\0'
        public char CharAscii_x_Value => '\x78';                    // x (lower case)
        public char CharAscii_Z_Value => '\x5A';                    // Z (uppder case)
        public char CharUnicode_P_Value => '\u0420';                // P (upper case)
        public char CharUnicode_C_Value => '\u0421';                // C (upper code)

        public DateTime DateTimeMinValue => DateTime.MinValue;      // 1/1/0001 12:00:00 AM
        public DateTime DateTimeMaxValue => DateTime.MaxValue;      // 12/31/9999 11:59:59 PM

        public StructExample StructExampleValue => new StructExample(1112, 2, false, 42);
        public EnumInt32Example EnumExampleValue => EnumInt32Example.Unknown;

        public (double, int) ValueTupleExamlpe => (4.5, 3);

        public FruitsTypes FruitsTypesExample =>
            FruitsTypes.Coconut
            | FruitsTypes.Cherries
            | FruitsTypes.Grapefruit;
    }
}

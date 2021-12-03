using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    internal class DemoCode
    {
        private void ArithmeticOperatorsExample()
        {
            int a, b = 53, c = -10, d = 0, e;               // a=0, b=53, c=-10, d=0, e=0
            // unary
            //a++;                                          // Compile error: Use of unassigned local variable 'a'   
            a = 45;                                         // a=45 
            a++;                                            // a=46
            a--;                                            // a=45
            d = a--;                                        // d=45, a=44
            e = --b;                                        // e=52, b=52
            a = -b;                                         // a=-52, b=52
            c = -(-b);                                      // b=52, c=52
            d = +a;                                         // a=-52, d=-52

            var a1 = short.MaxValue;                        // int16 a1=32767
            var a2 = 32767;                                 // int32 a2=32767
            var b1 = a2 == short.MaxValue;                  // bool b1=true (short.MaxValue int16 -> int32)
            var b2 = a1++;                                  // int16 b2=32767, int16 a1=-32768
            var b3 = a2++;                                  // int32 a2=32768, int32 b3=32767

            char f1 = 'b';                                  // char='b'
            var g1 = (int)f1;                               // int32 g1=98    
            char f2 = f1++;                                 // f1='c', f2='b' (char -> int32++)
            var g2 = (int)f2;                               // int32 g2=98
            char f3 = ++f1;                                 // f3='d', f1='d' (char -> ++int32)
            var g3 = (int)f3;                               // 100

            // binary
            var h1 = f1 * 3;                                // int32 h1=300
            var h2 = g3 * 3;                                // int32 h2=300

            var i1 = d / 2;                                 // int32 i1=-26
            var j1 = d % 3;                                 // int32 j1= -1, d=-52
            var j2 = b % 5;                                 // int32 j2=2
            float j3 = b % 5;                               // float j3=2 (|52|-10*|5|)
            float j4 = b / 5;                               // float j4=10
            float j5 = (float)b % -5;                       // int32 j5=2 (|52|-10*|-5|)
            float j6 = (float)b / 5;                        // float j6=10.4
            var j7 = Math.DivRem(b, 5, out c);              // int32 j7=10, c=2, b=52

            var k1 = j1 + j2;                               // int32 k1=1
            var k2 = j1 - j2;                               // int32 k2=-3
            var k3 = 2 * 2 - 3;                             // int32 k3=1
            var k4 = 2 * (2 - 3);                           // int32 k4=-2
            var k5 = 1 + (2 * (6 - 3)) * (5 - 3) + 2;       // int32 k5=15
        }


        private void ComparisonOperatorsExample()
        {
            int n1 = 5, n2 = 6, n3 = 5;
            bool a1 = n1 == n2;                     // false
            bool a2 = n2 != n3;                     // true
            bool a4 = n1 < n2;                      // true
            bool a5 = n2 >= n1;                     // true

            bool b1 = a1 && a2;                     // false
            bool b2 = n1 == n2 && n2 != n3;         // false
            bool b3 = a1 || a2;                     // true
            bool b4 = !a1;                          // true
        }

        private void NullCoalesingExample()
        {
            string[] example1 = new[] { "a", null, "c", "d" };
            string nullValue = example1[1];                                     // null
            string notNullValue = example1[2];                                  // "c"
            string p1 = nullValue ?? "b";
            string p2 = string.IsNullOrEmpty(notNullValue) ? "s" : "q";         // "q"
            int? p3 = nullValue?.Length;                                        // null
            //int p4 = nullValue.Length;                                        // throws NullReferenceException
            char? p5 = notNullValue?[0];                                        // 'c'
            char? p6 = nullValue?[0];                                           // null
            char p7 = nullValue[0];
        }

        private void EnumFlagExample()
        {
            // also see Common/Enums/Info.txt
            var monday1 = Day.Monday;
            var monday2 = DayFlags.Monday;
            var monday3 = DayFlagInt32.Monday;
            var monday4 = DayFlagBitLiteral.Monday;

            var saturdayInt32M1 = (int)Day.Saturday;                                // 6
            var saturdayInt32M2 = (int)DayFlags.Saturday;                           // 32
            var saturdayInt32M3 = (int)DayFlagInt32.Saturday;                       // 32
            var saturdayInt32M4 = (int)DayFlagBitLiteral.Saturday;                  // 32

            var mondyOrFriday1 = DayFlags.Monday | DayFlags.Friday;

            var checkFridayInWeekdays = (DayFlags.Weekdays & DayFlags.Friday) != 0; // true

            var sb = new StringBuilder();
            sb.AppendLine("All possible Day variants");
            for (int d = 0; d < 10; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (Day)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayFlags variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayFlags)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayFlagInt32 variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayFlagInt32)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayFlagBitLiteral variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayFlagBitLiteral)d, Environment.NewLine);
            }

            sb.AppendLine();
            sb.AppendLine("All possible DayInt32Pow2 variants");
            for (int d = 0; d < 128; d++)
            {
                sb.AppendFormat("{0,3} - {1:G} {2}", d, (DayInt32Pow2)d, Environment.NewLine);
            }
            var info = sb.ToString();
        }

        private void ExpressionExample()
        {
            int n1, n2, n3;             // n1:0, n2:0, n3:0
            n1 = 12;                    // n1:12, n2:0, n3:0
            n2 = n1;                    // n1:12, n2:12, n3:0
            n3 = n1 * ++n2;             // n1*(n2=n2+1), n1:12, n2:13, n3:156
            n3 = n1 * n2++;             // n1*n2, n2=n2+1, n1:12, n2:14, n3:156

            n3 = 1 + 2 * 3;             // 7
            n3 = (1 + 2) * 3;           // 9

            int s1, s2, s3;
            s1 = 10;                    //  0000 0000 0000 1010
            s2 = s1 >> 1;               //  0000 0000 0000 0101
            s3 = s1 << 1;               //  0000 0000 0001 0100

            int z1, z2, z3;
            z1 = 25;                    // 0000 0000 0001 1001 = 25
            z2 = 72;                    // 0000 0000 0100 1000 = 72
            z3 = z1 & z2;               // 0000 0000 0000 1000 = 8
            z3 = z1 | z2;               // 0000 0000 0101 1001 = 89
            z3 = z1 ^ z2;               // 0000 0000 0101 0001 = 81

            z3 = ~z1;                   // 11111111111111111111111111111111111111111111111111111111 1110 0110 = -26

            uint a = 0b_1010_0000;      // 1010 0000 = 160
            uint b = 0b_1001_0001;      // 1001 0001 = 145
            uint c1 = a | b;            // 1011 0001 = 177
            uint c2 = 0b_1011_0001;     // 177
            var d = c1 == c2;           // true
        }
    }
}

using System;

namespace ArithmeticOperatorsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ArithmeticOperatorsExample");

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
    }
}

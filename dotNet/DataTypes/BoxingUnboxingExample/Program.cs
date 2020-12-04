using System;

namespace BoxingUnboxingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BoxingUnboxingExample");
            try
            {
                int a1 = 10101;                 // System.Int32
                long a2 = 10101;                // System.Int64
                object b1 = a1;                 // boxing a1
                object b2 = a2;                 // boxing a2
                object b3 = 10101;              // boxing of System.Int32

                int c1 = (int)b1;               // unboxing b1
                //int c2 = (int)b2;             // Unable to cast object of type 'System.Int64' to type 'System.Int32'
                int c22 = (int)(long)b2;
                int c3 = (int)b3;               // unboxing b1

                //long d1 = (long)b1;           // Unable to cast object of type 'System.Int32' to type 'System.Int64'.
                long d2 = (long)b2;             // unboxing b2
                                                //long d3 = (long)b3; // Unable to cast object of type 'System.Int32' to type 'System.Int64'.

                //byte e1 = (byte)b1;           // Unable to cast object of type 'System.Int32' to type 'System.Byte'.
                //DateTime f1 = (DateTime)b1;   // Unable to cast object of type 'System.Int32' to type 'System.DateTime'.
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("New InvalidCastException");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("New general Exception");
                Console.WriteLine(e.Message);
            }
        }
    }
}

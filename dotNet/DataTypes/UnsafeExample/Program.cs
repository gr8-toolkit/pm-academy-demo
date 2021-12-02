using System;
using System.Runtime.InteropServices;

namespace UnsafeExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var str = "Hello World!";
            var reversedStr = ReverseStringUnsafe(str);


            Console.WriteLine();
        }


        static string ReverseStringUnsafe(string str)
        {
            var strLength = str.Length;

            // copy str content to unmanaged memory
            IntPtr sourcePtr = Marshal.StringToHGlobalAnsi(str);
            // allocate unmanaged {strLength + 1} bytes
            IntPtr destinationPtr = Marshal.AllocHGlobal(strLength + 1);

            unsafe
            {
                byte* src = (byte*)sourcePtr.ToPointer();
                byte* dst = (byte*)destinationPtr.ToPointer();


                if (strLength > 0)
                {
                    // set the source pointer to the end of the string
                    // to do a reverse copy.
                    src += strLength - 1;

                    while (strLength-- > 0)
                    {
                        *dst++ = *src--;
                    }

                    *dst = 0;

                }
            }

            var reversedStr = Marshal.PtrToStringAnsi(destinationPtr);

            // Free HGlobal memory
            Marshal.FreeHGlobal(destinationPtr);
            Marshal.FreeHGlobal(sourcePtr);

            return reversedStr;
        }

    }
}

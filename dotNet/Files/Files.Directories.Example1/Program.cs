using System;
using System.IO;

namespace Files.Directories.Example1
{
    class Program
    {
        static void Main()
        {
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current dir : {dir}");
        }
    }
}

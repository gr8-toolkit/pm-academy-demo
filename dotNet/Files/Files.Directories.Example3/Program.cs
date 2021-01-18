using System;
using System.IO;

namespace Files.Directories.Example3
{
    class Program
    {
        static void Main()
        {
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current dir : {dir}");

            var path = @"SubDir1\SubDir2\SubDir3";
            Console.WriteLine($"Ensure path : {path}");
            Directory.CreateDirectory(path);
        }
    }
}

using System;
using System.IO;

namespace Files.Directories.Example3
{
    /// <summary>
    /// Demo for <see cref="Directory.CreateDirectory"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
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

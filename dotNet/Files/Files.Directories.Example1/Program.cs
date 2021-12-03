using System;
using System.IO;

namespace Files.Directories.Example1
{
    /// <summary>
    /// Demo for <see cref="Directory.GetCurrentDirectory"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        private static void Main()
        {
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current dir : {dir}");
        }
    }
}

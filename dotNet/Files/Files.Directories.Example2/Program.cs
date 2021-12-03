using System;
using System.IO;

namespace Files.Directories.Example2
{
    /// <summary>
    /// Demo for <see cref="Directory.EnumerateFiles"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        private static void Main()
        {
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine("Current directory {0} :", dir);
            foreach (var file in Directory.EnumerateFiles(dir, "*.dll"))
            {
                Console.WriteLine(Path.GetFileName(file));
            }
        }

    }
}

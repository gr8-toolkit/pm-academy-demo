using System;
using System.IO;

namespace Files.Directories.Example4
{
    /// <summary>
    /// Demo for <see cref="Directory.Delete"/>.
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

            var path = "SubDir1";
            Console.WriteLine($"Ensure path : {path}");

            Directory.CreateDirectory(path);
            Console.WriteLine("Dir {0} exists: {1}", path, Directory.Exists(path)); 

            Directory.Delete(path, recursive: true);
            Console.WriteLine("Dir {0} exists: {1}", path, Directory.Exists(path));
        }
    }
}

using System;
using System.IO;

namespace Files.Directories.Example5
{
    /// <summary>
    /// Demo for <see cref="Directory.Move"/>.
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

            Directory.Move(path, path + "_copy");
            Console.WriteLine("Dir {0} exists: {1}", path + "_copy", Directory.Exists(path + "_copy"));

            Directory.Delete(path + "_copy");
        }
    }
}

using System;
using System.IO;

namespace Files.Directories.Example2
{
    class Program
    {
        static void Main()
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

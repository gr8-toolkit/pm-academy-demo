using System;
using System.IO;
using System.Linq;

namespace Files.Directories.Example6
{
    /// <summary>
    /// Simple command line interpreter.
    /// Demo for <see cref="Directory"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            PrintHelp();

            while (true)
            {
                var current = Directory.GetCurrentDirectory();
                Console.Write($"{current} > ");
                
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                
                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts.First();
                var arg = parts.Last();
                
                switch (cmd)
                {
                    case "exit" : 
                        return;
                    case "?": 
                        PrintHelp();
                        continue;
                    case "dir":
                        Console.WriteLine();
                        foreach (var entry in Directory.EnumerateFileSystemEntries("."))
                        {
                            Console.WriteLine(Path.GetFileName(entry));
                        }
                        break;
                    case "cd..":
                    case "cd" when arg == "..":
                        var parent = Directory.GetParent(current);
                        if (parent != null) Directory.SetCurrentDirectory(parent.FullName);
                        break;
                    case "cd" when !string.IsNullOrEmpty(arg):
                        if (!Directory.Exists(arg)) continue;
                        Directory.SetCurrentDirectory(Path.Combine(current, arg));
                        break;
                    default: 
                        continue;
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Simple cmd");
            Console.WriteLine("exit - for exit");
            Console.WriteLine("? - help");
            Console.WriteLine("cd .. - go up");
            Console.WriteLine("cd <sub dir> - drill down");
        }
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Files.SysIo.Example1
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("System.IO demo");
            
            DirectoryAndPathDemo();
            FileDemo();
            DriveDemo();
            FileStreamDemo("timelog.txt");
            StreamReaderDemo("timelog.txt");
        }
        
        private static void DirectoryAndPathDemo()
        {
            Console.WriteLine();
            Console.WriteLine("Directory and path demo");

            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine("Current directory {0} :", dir);
            foreach (var entry in Directory.EnumerateFileSystemEntries(dir))
            {
                Console.WriteLine(Path.GetFileName(entry));
            }
        }

        private static void FileDemo()
        {
            Console.WriteLine();
            Console.WriteLine("File demo");

            // Current executing assembly (library)
            var file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"Current assembly : {file}");
            Console.WriteLine($"Attributes : {file.Attributes}");
            Console.WriteLine($"Length (bytes) : {file.Length}");
        }

        private static void DriveDemo()
        {
            Console.WriteLine();
            Console.WriteLine("Drive demo");

            var drives = DriveInfo.GetDrives();

            foreach (var d in drives)
            {
                Console.WriteLine("Drive {0} [{1}]", d.Name, d.DriveType);
            }
        }


        private static void FileStreamDemo(string fileName)
        {
            Console.WriteLine();
            Console.WriteLine("File stream demo");

            // Use `using` keyword to flush changes
            using var stream = new FileStream(fileName, FileMode.Append);
            var time = $"{DateTime.Now:F}\n";
            var bytes = Encoding.UTF8.GetBytes(time);

            // Append file
            stream.Write(bytes, 0, bytes.Length);

            Console.WriteLine($"Time-log {fileName} was updated");
        }

        private static void StreamReaderDemo(string fileName)
        {
            Console.WriteLine();
            Console.WriteLine("Stream reader demo");

            using var stream = new FileStream(fileName, FileMode.Open);
            using var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            Console.WriteLine($"Content of {fileName} :");
            Console.WriteLine(text);
        }
    }
}
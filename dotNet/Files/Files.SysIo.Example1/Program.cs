using System;
using System.IO;

namespace Files.SysIo.Example1
{
    /// <summary>
    /// Demo program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// App entry point.
        /// </summary>
        private static void Main()
        {
            Console.WriteLine("System.IO demo");

            DirectoryAndPathDemo();
            FileDemo();
            DriveDemo();
            FileStreamDemo("timelog.txt");
            StreamReaderDemo("timelog.txt");
        }
        
        /// <summary>
        /// Gets current application directory and list all inner entries.
        /// </summary>
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

        /// <summary>
        /// Creates file abstraction for current assembly.
        /// Prints assembly file attributes.
        /// </summary>
        private static void FileDemo()
        {
            Console.WriteLine();
            Console.WriteLine("File demo");

            // Current executing assembly (library)
            var assemblyFile = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"Current assembly : {assemblyFile}");
            Console.WriteLine($"Attributes : {assemblyFile.Attributes}");
            Console.WriteLine($"Length (bytes) : {assemblyFile.Length}");
        }

        /// <summary>
        /// Prints attached drives info.
        /// </summary>
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

        /// <summary>
        /// Appends current time string to time-log file.
        /// Demo is based on <see cref="FileStream"/>.
        /// </summary>
        /// <param name="fileName">Time-log file name.</param>
        private static void FileStreamDemo(string fileName)
        {
            Console.WriteLine();
            Console.WriteLine("File stream demo");

            // Use `using` keyword to flush changes
            using var stream = new FileStream(fileName, FileMode.Append);
            var currentTimeString = $"{DateTime.Now:F}\n";
            var currentTimeStringBytes = System.Text.Encoding.UTF8.GetBytes(currentTimeString);

            // Append file
            stream.Write(currentTimeStringBytes, 0, currentTimeStringBytes.Length);

            Console.WriteLine($"Time-log {fileName} was updated");
        }

        /// <summary>
        /// Prints file content to console.
        /// Demo is based on text <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="fileName">File with content.</param>
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
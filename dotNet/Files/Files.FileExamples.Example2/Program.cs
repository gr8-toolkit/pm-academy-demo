using System;
using System.IO;

namespace Files.FileExamples.Example2
{
    /// <summary>
    /// Demo for <see cref="File"/>.
    /// Demo for <see cref="File.Delete"/> and <see cref="IOException"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        private static void Main()
        {
            var time = $"{DateTime.Now:F}\n";

            File.AppendAllText("timelog.txt", time);
            Console.WriteLine("Time-log was updated");

            File.Delete("timelog.txt");

            var text = File.ReadAllText("timelog.txt");
            Console.WriteLine("Timelog content :");
            Console.WriteLine(text);
        }
    }
}

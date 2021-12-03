using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Files.FileExamples.Example1
{
    /// <summary>
    /// Demo for <see cref="File"/>.
    /// For <see cref="File.AppendAllText"/> and <see cref="File.ReadAllText"/>.
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

            var text = File.ReadAllText("timelog.txt");
            Console.WriteLine("Timelog content :");
            Console.WriteLine(text);
        }
    }
}

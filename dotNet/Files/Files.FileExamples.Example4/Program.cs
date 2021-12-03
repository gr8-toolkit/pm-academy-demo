using System;
using System.IO;

namespace Files.FileExamples.Example4
{
    /// <summary>
    /// Demo for <see cref="File"/>.
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

            using var stream = File.Open("timelog.txt", FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);

            Console.WriteLine("Timelog binary content :");
            while (stream.Position < stream.Length)
            {
                var bt = reader.ReadByte();
                Console.Write($"{bt:X2}");
            }
        }
    }
}

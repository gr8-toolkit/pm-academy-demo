using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Files.FileExamples.Example5
{
    /// <summary>
    /// Demo for <see cref="File"/> async API.
    /// For <see cref="File.AppendAllTextAsync"/> and <see cref="File.ReadAllTextAsync"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        private static async Task Main()
        {
            var time = $"{DateTime.Now:F}\n";

            await File.AppendAllTextAsync("timelog.txt", time);
            Console.WriteLine("Time-log was updated asynchronously");

            var text = await File.ReadAllTextAsync("timelog.txt");
            Console.WriteLine("Timelog content :");
            Console.WriteLine(text);
        }
    }
}

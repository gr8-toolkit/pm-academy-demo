using System;
using System.IO;

namespace Files.FileExamples.Example2
{
    class Program
    {
        static void Main()
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

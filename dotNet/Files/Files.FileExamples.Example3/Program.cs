using System;
using System.IO;

namespace Files.FileExamples.Example3
{
    class Program
    {
        static void Main()
        {
            var time = $"{DateTime.Now:F}\n";
            File.AppendAllText("timelog.txt", time);
            Console.WriteLine("Time-log was updated");

            Console.WriteLine("Timelog content :");
            var lineNumber = 0;

            foreach (var line in File.ReadLines("timelog.txt"))
            {
                lineNumber++;
                Console.WriteLine($"{lineNumber} : {line}");
            }
        }
    }
}

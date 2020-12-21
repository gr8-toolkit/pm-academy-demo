using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Files.FileExamples.Example1
{
    class Program
    {
        static void Main()
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

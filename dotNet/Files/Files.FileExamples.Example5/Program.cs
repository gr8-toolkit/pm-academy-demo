using System;
using System.IO;
using System.Linq;

namespace Files.FileExamples.Example5
{
    class Program
    {
        static void Main(string[] args)
        {
            // useful for logs with lot of \n

            Console.WriteLine(@"File \n splitter");
            var inputFile = args.FirstOrDefault() ?? "input.txt";
            
            if (string.IsNullOrEmpty(inputFile) || !File.Exists(inputFile))
            {
                Console.WriteLine($"Invalid input file {inputFile}");
                return;
            }

            using var stream = File.OpenWrite("result.txt");
            using var writer = new StreamWriter(stream);
            foreach (var line in File.ReadLines(inputFile))
            {
                foreach (var split in line.Split(@"\n"))
                {
                    writer.WriteLine(split);
                }
                // Write all data immediately - just for Flush demo
                writer.Flush();
            }

            // Alternative way with LINQ
            //var splits = File.ReadLines(inputFile).SelectMany(line => line.Split(@"\n"));
            //foreach (var split in splits)
            //{
            //    writer.WriteLine(split);
            //}
        }
    }
}

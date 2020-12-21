using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Files.Streams.Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = args.Length > 0 ? args[0] : "input.txt";
            var output = args.Length > 1 ? args[1] : "output.gz";
            Console.WriteLine($"Input '{input}' compress to {output}");
            
            Compress(input, output);
            
            Console.WriteLine("Done !");
        }

        private static void Compress(string input, string output)
        {
            using var inputStream = File.OpenRead(input);
            using var outputStream = File.Create(output);

            using var zipStream = new GZipStream(outputStream, CompressionMode.Compress);
            inputStream.CopyTo(zipStream);
        }
    }
}

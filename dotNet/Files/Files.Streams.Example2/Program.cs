using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Files.Streams.Example2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("String to compress : ");
            var input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                input = Console.ReadLine();
            }

            Compress(input, "output.gz");

            Console.WriteLine("Done !");
        }

        private static void Compress(string inputString, string outputFile)
        {
            var bytes = Encoding.UTF8.GetBytes(inputString);
            using var inputStream = new MemoryStream(bytes);
            using var outputStream = File.Create(outputFile);

            using var zipStream = new GZipStream(outputStream, CompressionMode.Compress);
            inputStream.CopyTo(zipStream);
        }
    }
}
using System;
using System.IO;
using System.Reflection;

namespace Files.Paths.Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            var location = Assembly.GetExecutingAssembly().Location;

            Console.WriteLine("Current assembly: {0}", Path.GetFileName(location));
            Console.WriteLine("has extension: {0}", Path.GetExtension(location));
            Console.WriteLine("at drive: {0}", Path.GetPathRoot(location));
            Console.WriteLine("in directory: {0}", Path.GetDirectoryName(location));
        }
    }
}

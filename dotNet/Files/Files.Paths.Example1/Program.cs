using System;
using System.IO;
using System.Reflection;

namespace Files.Paths.Example1
{
    /// <summary>
    /// Demo for <see cref="Path"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        private static void Main()
        {
            var location = Assembly.GetExecutingAssembly().Location;

            Console.WriteLine("Current assembly: {0}", Path.GetFileName(location));
            Console.WriteLine("has extension: {0}", Path.GetExtension(location));
            Console.WriteLine("at drive: {0}", Path.GetPathRoot(location));
            Console.WriteLine("in directory: {0}", Path.GetDirectoryName(location));
        }
    }
}

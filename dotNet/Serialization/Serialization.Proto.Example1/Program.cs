using System;
using System.IO;
using Google.Protobuf;

namespace Serialization.Proto.Example1
{
    /// <summary>
    /// Google protobuf demo.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Hello World!");
            var person = new Person {Name = "Max", Age = 29};

            //Create memory stream to keep serialized data
            using var memStream = new MemoryStream();
            using var stream = new CodedOutputStream(memStream);

            // Write person data to the stream
            person.WriteTo(stream);
            stream.Flush();
            
            // Get bytes from person data stream
            var bytes = memStream.ToArray();
            
            // Create person copy from origin bytes
            var clone = Person.Parser.ParseFrom(bytes);

            Console.WriteLine($"Source: {person}; clone: {clone}");
        }
    }
}

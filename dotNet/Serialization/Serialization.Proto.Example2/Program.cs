using System;
using System.IO;
using ProtoBuf;
using Serialization.Data;

namespace Serialization.Proto.Example2
{
    /// <summary>
    /// Protobuf-net demo.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            // Creates person to clone
            var person = new Person("Max", 29);

            //Creates memory stream to keep serialized data
            using var stream = new MemoryStream();
            
            // Serialize person to stream
            Serializer.Serialize(stream, person);
            
            // Move stream cursor to beginning
            stream.Seek(0, SeekOrigin.Begin);

            // Create person copy from origin bytes
            var clone = Serializer.Deserialize<Person>(stream);
            person.Name = "New name";
            Console.WriteLine($"Source: {person}; clone: {clone}");
        }
    }
}

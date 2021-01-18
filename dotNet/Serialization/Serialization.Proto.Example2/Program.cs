using System;
using System.IO;
using ProtoBuf;
using Serialization.Data;

namespace Serialization.Proto.Example2
{
    class Program
    {
        static void Main()
        {
            var person = new Person("Max", 29);
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, person);
            stream.Seek(0, SeekOrigin.Begin);
            var clone = Serializer.Deserialize<Person>(stream);
            person.Name = "New name";
            Console.WriteLine($"Source: {person}; clone: {clone}");
        }
    }
}

using System;
using System.IO;
using Google.Protobuf;

namespace Serialization.Proto.Example1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            var person = new Person {Name = "Max", Age = 29};

            using var memStream = new MemoryStream();
            using var stream = new CodedOutputStream(memStream);

            person.WriteTo(stream);
            stream.Flush();
            var bytes = memStream.ToArray();
            var clone = Person.Parser.ParseFrom(bytes);

            Console.WriteLine($"Source: {person}; clone: {clone}");
        }
    }
}

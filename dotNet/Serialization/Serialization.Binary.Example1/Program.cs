using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Serialization.Data;

namespace Serialization.Binary.Example1
{
    class Program
    {
        static void Main()
        {
            var objects = new object[] {"some string", new Exception("Random error"), 42};
            var index = new Random().Next(0, objects.Length);

            using var stream = Serialize(objects[index]);
            
            var something = Deserialize(stream);
            Console.WriteLine($"It was : {something}");
        }

        private static Stream Serialize(object graph)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, graph);
            return stream;
        }

        private static object Deserialize(Stream stream)
        {
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            
            // DANGER! What kind of data in the stream ???
            return formatter.Deserialize(stream);
        }
    }
}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Serialization.Data;

#pragma warning disable SYSLIB0011 // BinaryFormatter is obsolete
namespace Serialization.Binary.Example1
{
    /// <summary>
    /// Binary formatter demo
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            var objects = new object[] {"some string", new Exception("Random error"), 42};
            var index = new Random().Next(0, objects.Length);

            // Deserialize random object
            using var stream = Serialize(objects[index]);
            
            var something = Deserialize(stream);
            Console.WriteLine($"It was : {something}");
        }

        /// <summary>
        /// Serialization with <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <remarks>
        /// BinaryFormatter serialization is obsolete and should not be used. 
        /// See https://aka.ms/binaryformatter for more information.
        /// </remarks>
        /// <returns>Returns binary stream with serialized object graph.</returns>
        private static Stream Serialize(object graph)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            formatter.Serialize(stream, graph);
            return stream;
        }

        /// <summary>
        /// Deserialization with <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <remarks>
        /// BinaryFormatter serialization is obsolete and should not be used. 
        /// See https://aka.ms/binaryformatter for more information.
        /// </remarks>
        /// <param name="stream">Binary stream with serialized object graph.</param>
        /// <returns>Returns deserialized (restored) object.</returns>
        private static object Deserialize(Stream stream)
        {
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            
            // DANGER! What kind of data in the stream ???
            return formatter.Deserialize(stream);
        }
    }
}
#pragma warning restore SYSLIB0011 // BinaryFormatter is obsolete

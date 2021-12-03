using System;
using System.IO;
using System.Xml.Serialization;
using Serialization.Data;

namespace Serialization.Xml.Example1
{
    /// <summary>
    /// <see cref="XmlSerializer"/> demo.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            // Create family structure
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);
            
            // Create serializer for Person
            var serializer = new XmlSerializer(typeof(Person));
            var writer = new StringWriter();

            // Serialize Person to StringWriter
            // Check other overloads of Serialize method
            serializer.Serialize(writer, son);
            
            // Print XML
            Console.WriteLine(writer);
        }
    }
}

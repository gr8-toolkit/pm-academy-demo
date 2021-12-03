using Serialization.Data;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Serialization.Xml.Example2
{
    /// <summary>
    /// LinqToXml demo.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            // Create data structure with few persons
            var people = new []
            {
                new Person("Nick", 23),
                new Person("Max", 29), 
                new Person("John", 18)
            };

            // Serialize data structre to XML
            var serializer = new XmlSerializer(typeof(Person[]));
            var writer = new StringWriter();
            serializer.Serialize(writer, people);
            var xml = writer.ToString();
            
            // Display XML
            Console.WriteLine(xml);
            Console.WriteLine();

            // Try to find firts person with Name == Max
            var root = XElement.Parse(xml);
            var elements = from p in root.Descendants("Person")
                let name = p.Descendants("Name").First()
                where name.Value == "Max" select p;
            
            // Display search results on console
            Console.WriteLine("Max :");
            Console.WriteLine($"{elements.First()}");
        }
    }
}

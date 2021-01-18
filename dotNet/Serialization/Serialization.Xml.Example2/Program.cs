using Serialization.Data;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Serialization.Xml.Example2
{
    class Program
    {
        static void Main()
        {
            var people = new []
            {
                new Person("Nick", 23),
                new Person("Max", 29), 
                new Person("John", 18)
            };
            var serializer = new XmlSerializer(typeof(Person[]));
            var writer = new StringWriter();
            serializer.Serialize(writer, people);
            var xml = writer.ToString();
            
            Console.WriteLine(xml);
            Console.WriteLine();

            var root = XElement.Parse(xml);
            var elements = from p in root.Descendants("Person")
                let name = p.Descendants("Name").First()
                where name.Value == "Max" select p;
            
            Console.WriteLine("Max :");
            Console.WriteLine($"{elements.First()}");
        }
    }
}

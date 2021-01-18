using System;
using System.IO;
using System.Xml.Serialization;
using Serialization.Data;

namespace Serialization.Xml.Example1
{
    class Program
    {
        static void Main()
        {
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);
            
            var serializer = new XmlSerializer(typeof(Person));
            var writer = new StringWriter();
            serializer.Serialize(writer, son);
            Console.WriteLine(writer);
        }
    }
}

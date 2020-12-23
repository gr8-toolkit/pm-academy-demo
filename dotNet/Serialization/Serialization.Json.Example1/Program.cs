using Serialization.Data;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serialization.Json.Example1
{
    class Program
    {
        static void Main()
        {
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Serialize
            var json = JsonSerializer.Serialize(son,options);
            Console.WriteLine(json);
            
            // Deserialize back
            var clone = JsonSerializer.Deserialize<Person>(json, options);
            Console.WriteLine(clone);
        }
    }
}

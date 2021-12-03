using Serialization.Data;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serialization.Json.Example3
{
    /// <summary>
    /// Standard System.Text.Json serialization/deserialization example.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            // Creates family tree
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);

            // JsonSerializer settings
            var options = new JsonSerializerOptions
            {
                // Use formatting with 'new line' and 'tab'
                WriteIndented = true,
                // Use camelCase naming for properties
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Serialize object to string
            var json = JsonSerializer.Serialize(son, options);
            Console.WriteLine(json);
            
            // Deserialize back
            var clone = JsonSerializer.Deserialize<Person>(json, options);
            Console.WriteLine(clone);
        }
    }
}

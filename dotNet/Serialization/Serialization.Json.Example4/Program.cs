using Serialization.Data;
using System;
using System.Text.Json;

namespace Serialization.Json.Example4
{
    /// <summary>
    /// Standard System.Text.Json JsonDocument example.
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

            // Creates JsonDocument from string
            // JsonDocument is immutable (read-only)
            var document = JsonDocument.Parse(json);

            // Tries to get value of 'root.parent1.fullName'
            // No JPath support (net5.0)
            var value = document
                .RootElement
                .GetProperty("parent1")
                .GetProperty("fullName")
                .GetString();

            // Writes property value to console
            Console.WriteLine("'root.parent1.fullName' = {0}", value);
        }
    }
}

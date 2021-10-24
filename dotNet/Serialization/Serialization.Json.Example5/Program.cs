using Serialization.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serialization.Json.Example5
{
    /// <summary>
    /// Contract with variable part - ExtensionData.
    /// </summary>
    internal class PersonExtended : Person
    {
        /// <summary>
        /// Extension data container.
        /// It will be filled with <see cref="System.Text.Json.JsonElement"/> 
        /// instead of pure <see cref="System.Object"/>.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }

    /// <summary>
    /// Standard System.Text.Json JsonExtensionData example.
    /// </summary>
    class Program
    {
        // JsonSerializer settings
        private static readonly JsonSerializerOptions options = new()
        {
            // Use formatting with 'new line' and 'tab'
            WriteIndented = true,
            // Use camelCase naming for properties
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Entry point.
        /// </summary>
        static void Main()
        {
            // Extended json
            var json = "{" +
                "\"fullName\": \"Son\"," +
                "\"age\": 29," +
                "\"intExtraProp\": 42," +
                "\"strExtraProp\": \"extra\"," +
                "\"objExtraProp\": {\"inner\": 96}" +
                "}";

            // Deserialize json
            var person = JsonSerializer.Deserialize<PersonExtended>(json, options);

            // Join ExtensionData as string
            var personExtenedData = string.Join(
                Environment.NewLine,
                person.ExtensionData.Select(kv => $"[{kv.Key}]='{kv.Value}'"));
            
            // Writes person content to console
            Console.WriteLine(person);
            Console.WriteLine(personExtenedData);

            // Serialize person back to JSON
            json = JsonSerializer.Serialize(person, options);
            Console.WriteLine(json);
        }
    }
}

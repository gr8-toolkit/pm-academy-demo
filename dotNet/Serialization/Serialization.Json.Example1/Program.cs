using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serialization.Data;

namespace Serialization.Json.Example1
{
    /// <summary>
    /// Newtonsoft JSON.NET serialization/deserialization example.
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

            // JsonConvert settings
            var settings = new JsonSerializerSettings
            {
                // Use formatting with 'new line' and 'tab'
                Formatting = Formatting.Indented,
                // Use camelCase naming for properties
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            // Serialize object to string
            var json = JsonConvert.SerializeObject(son, settings);
            Console.WriteLine(json);

            // Deserialize back
            var clone = JsonConvert.DeserializeObject<Person>(json, settings);
            Console.WriteLine(clone);
        }
    }
}

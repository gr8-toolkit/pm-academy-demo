using System;
using Newtonsoft.Json;
using Serialization.Data;

namespace Serialization.Intro
{
    /// <summary>
    /// JSON Serialization demo.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Create family data structure
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);

            // Serialize son object with relations
            ToJson(son);
        }

        private static void ToJson(Person son)
        {
            Console.WriteLine();
            Console.WriteLine("{0} to JSON", son);

            // Serialize object to JSON
            var json = JsonConvert.SerializeObject(son, Formatting.Indented);
            
            // Display JSON on console
            Console.WriteLine(json);
        }
    }
}

using System;
using System.Text.Json;

namespace Serialization.Json.Example6
{
    /// <summary> Key-Value pair with integer key.</summary>
    internal class KeyValueClass
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public override string ToString() => $"[{Key}]='{Value}'";
    }

    /// <summary> Key-Value pair with integer key.</summary>
    internal record KeyValueRecord(int Key, string Value);

    /// <summary> Key-Value pair with integer key.</summary>
    internal readonly struct KeyValueStruct
    {
        public int Key { get; init; }
        public string Value { get; init; }
        public override string ToString() => $"[{Key}]='{Value}'";
    }

    /// <summary>
    /// Class, Record, Struct serialization demo.
    /// Based on standard System.Text.Json.
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
            // Init demo data
            var class1 = new KeyValueClass() { Key = 1, Value = "stringValue1" };
            var rec1 = new KeyValueRecord(2, "stringValue2");
            var struct1 = new KeyValueStruct() { Key = 3, Value = "stringValue3" };

            // Serialize class, record, struct to string
            var classJson = JsonSerializer.Serialize(class1, options);
            var recJson = JsonSerializer.Serialize(rec1, options);
            var structJson = JsonSerializer.Serialize(struct1, options);
            
            Console.WriteLine("Class : {0}", classJson);
            Console.WriteLine("Record : {0}", recJson);
            Console.WriteLine("Struct : {0}", structJson);

            // Deserialize back with the SAME serializer options
            class1 = JsonSerializer.Deserialize<KeyValueClass>(classJson, options);
            rec1 = JsonSerializer.Deserialize<KeyValueRecord>(recJson, options);
            struct1 = JsonSerializer.Deserialize<KeyValueStruct>(structJson, options);

            Console.WriteLine("Class : {0}", class1);
            Console.WriteLine("Record : {0}", rec1);
            Console.WriteLine("Struct : {0}", struct1);
        }
    }
}

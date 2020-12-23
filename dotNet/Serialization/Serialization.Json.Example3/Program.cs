using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serialization.Data;

namespace Serialization.Json.Example3
{
    class Program
    {
        static void Main()
        {
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

            var json = JsonConvert.SerializeObject(son, settings);
            Console.WriteLine(json);

            var jObject = JObject.Parse(json);
            jObject["fullName"] = "New name";
            
            Console.WriteLine(jObject.ToString(Formatting.Indented));
        }
    }
}
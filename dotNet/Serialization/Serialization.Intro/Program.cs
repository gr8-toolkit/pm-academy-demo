using System;
using Newtonsoft.Json;
using Serialization.Data;

namespace Serialization.Intro
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var mother = new Person("Mom", 49);
            var father = new Person("Dad", 50);
            var son = new Person("Son", 29, mother, father);

            ToJson(son);
        }

        private static void ToJson(Person son)
        {
            Console.WriteLine();
            Console.WriteLine("{0} to JSON", son);

            var json = JsonConvert.SerializeObject(son, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}

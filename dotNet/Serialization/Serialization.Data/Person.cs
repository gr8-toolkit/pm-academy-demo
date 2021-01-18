using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Serialization.Data
{
    [Serializable]
    [DataContract]
    public class Person
    {
        [JsonPropertyName("fullName")]
        [DataMember(Order = 1, Name = "fullName")]
        public string Name { get; set; }
        
        [DataMember(Order = 2)]
        [JsonProperty("years")]
        public int Age { get; set; }
        
        [DataMember(Order = 3)]
        public Person Parent1 { get; set; }
        
        [DataMember(Order = 4)]
        public Person Parent2 { get; set; }

        public Person()
        {
        }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public Person(string name, int age, Person parent1, Person parent2)
        {
            Name = name;
            Age = age;
            Parent1 = parent1;
            Parent2 = parent2;
        }

        public override string ToString()
        {
            return $"{Name} [{Age}]";
        }
    }
}

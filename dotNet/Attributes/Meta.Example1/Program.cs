using System;
using System.Collections.Generic;
using System.Linq;

namespace Meta.Example1
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class ToStringAttribute : Attribute
    {
        private readonly string[] _props;

        public IEnumerable<string> Properties => _props;

        public ToStringAttribute(params string[] props)
        {
            _props = props ?? throw new ArgumentNullException(nameof(props));
        }
    }

    public static class ToStringExtension
    {
        /// <summary>
        /// String extension method for <see cref="ToStringAttribute"/>.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <returns>Returns target object string representation</returns>
        public static string ToStringExt(this object target)
        {
            if (target == null) return null;

            // Try to find ToStringAttribute for target type
            Type type = target.GetType();
            var attribute = Attribute.GetCustomAttribute(type, typeof(ToStringAttribute)) as ToStringAttribute;

            // Can't find ToStringAttribute or attribute is corrupted - return null 
            if (attribute?.Properties == null) return target.ToString();

            // Select properties and values
            var properties = attribute.Properties
                .Select(propName => type.GetProperty(propName))
                .Where(prop => prop != null)
                .Select(prop =>  new
                {
                    PropName = prop.Name,
                    PropValue = prop.GetValue(target).ToStringExt()
                })
                .Where(x => x.PropValue != null)
                .Select(x => $"{x.PropName}:{x.PropValue}");
            
            // Format output
            return $"[{String.Join(',', properties)}]";
        }
    }


    [ToString(nameof(Person.Name), nameof(Person.Age) , nameof(Person.Father), nameof(Person.Mother))]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person Father { get; set; }
        public Person Mother { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var max = new Person
            {
                Age = 29, Name = "Max",
                Father = new Person { Age = 51, Name = "John"},
                Mother = new Person { Age = 50, Name = "Anne" }
            };

            // Stack overflow
            //max.Father.Father = max;

            Console.WriteLine($"Hello {max.ToStringExt()}");
        }
    }
}

using Serialization.Data;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialization.Binary.Example2
{
    [Serializable]
    public class PersonCloneable : Person
    {
        public PersonCloneable(string name, int age)
            : base(name, age)
        {
        }

        public PersonCloneable(string name, int age, Person parent1, Person parent2)
            : base(name, age, parent1, parent2)
        {
        }

        /// <summary>
        /// Shallow clone (1st level fields only).
        /// </summary>
        public PersonCloneable Clone()
        {
            return (PersonCloneable)this.MemberwiseClone();
        }

        /// <summary>
        /// Deep copy of object graph with <see cref="BinaryFormatter"/>.
        /// </summary>
        /// <remarks>
        /// BinaryFormatter serialization is obsolete and should not be used. 
        /// See https://aka.ms/binaryformatter for more information.
        /// </remarks>
        public PersonCloneable DeepClone()
        {
            using var stream = new MemoryStream();
            var formatter = new BinaryFormatter();

            #pragma warning disable SYSLIB0011 // BinaryFormatter is obsolete
            
            formatter.Serialize(stream, this);
            
            stream.Seek(0, SeekOrigin.Begin);
            var clone = formatter.Deserialize(stream);
            return (PersonCloneable)clone;

            #pragma warning restore SYSLIB0011 // BinaryFormatter is obsolete
        }
    }

    class Program
    {
        static void Main()
        {
            var mother = new PersonCloneable("Mom", 49);
            var father = new PersonCloneable("Dad", 50);
            var origin = new PersonCloneable("Son", 29, mother, father);
            var clone = origin.Clone();
            var deepClone = origin.DeepClone();

            origin.Parent1.Name = "NewName";
            Console.WriteLine(
                $"origin.Parent1: {origin.Parent1}, " +
                $"clone.Parent1: {clone.Parent1}, " +
                $"deepClone.Parent1: {deepClone.Parent1}");
        }
    }
}

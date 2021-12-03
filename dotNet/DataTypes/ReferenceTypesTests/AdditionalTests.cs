using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataTypesTests
{
    public class AdditionalTests
    {
        private const string InitialStr = "initial";
        private const string ModifStr = "modified";
        private const int InitialNumb = 31;
        private const int ModifNumb = 51;

        [Fact]
        public void Value_Tuple()
        {
            // Act
            (int, string) example1 = (InitialNumb, InitialStr);
            (int numb, string text) example2 = (InitialNumb, "other");
            var example3 = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            (int a, int b) example4 = new ValueTuple<int, int>(InitialNumb, 331);
            (int param1, string param2) = (InitialNumb, InitialStr);

            // Action
            example2.text = ModifStr;
            example3.Item9 = ModifNumb;                            // 9 >> 51

            // Assert
            Assert.Equal(InitialNumb, example1.Item1);
            Assert.Equal(InitialNumb, example2.numb);
            Assert.Equal(ModifStr, example2.text);                 // ValueTuple is a mutable Struct
            Assert.Equal(ModifNumb, example3.Item9);               // first index = 1
            Assert.Equal(InitialNumb, example4.a);
            Assert.Equal(InitialNumb, param1);
            Assert.Equal(InitialStr, param2);
        }

        [Fact]
        public void Reference_Tuple()
        {
            // Act
            Tuple<int, string> example1 = new Tuple<int, string>(InitialNumb, InitialStr);
            Tuple<int, string> example2 = null;
            var example3 = Tuple.Create<int, string>(InitialNumb, InitialStr);
            var example4 = Tuple.Create<int, string>(InitialNumb, InitialStr);

            // Action
            //example4.Item2 = ModifStr;                            // not allowed for read only properties
            //example4 = Tuple.Create(ModifNumb, ModifNumb);        // not allowed because type safe
            example4 = Tuple.Create(ModifNumb, ModifStr);           // allocate new memory in the heap

            // Assert
            Assert.NotNull(example1);
            Assert.Null(example2);
            Assert.NotNull(example3);
            Assert.NotNull(example4);
            Assert.Equal(InitialNumb, example1.Item1);
            Assert.Equal(InitialStr, example1.Item2);
            Assert.Equal(ModifNumb, example4.Item1);
            Assert.Equal(ModifStr, example4.Item2);

        }
    }
}

using Common.Equality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataOperationsTests
{
    public class EqualityOperatorsTests
    {
        [Fact]
        public void Value_Types_Equality()
        {
            // Act
            int a, b;
            char c, d, e;

            // Action
            a = 1 + 2 + 3;
            b = 6;
            c = 'a';
            d = 'A';
            e = char.ToLower(d);

            // Assert
            Assert.True(a == b);
            Assert.False(c == d);
            Assert.True(c == e);
        }

        [Fact]
        public void Reference_Types_Equality()
        {
            // Act
            IExample example1 = new ExampleClass();
            IExample example2 = new ExamlleStruct();

            // Action
            example1.Execute();
            example2.Execute();

            // Assert
            Assert.NotNull(example1);
            Assert.NotNull(example2);
        }

        [Fact]
        public void String_Equality()
        {
            // Act
            string example1, example2, example3, example4, example5;

            // Action
            example1 = "hello";
            example2 = "heLLo";
            example3 = null;
            example4 = example1;
            example5 = example2.ToLower();

            // Assert
            Assert.NotNull(example1);
            Assert.NotNull(example2);
            Assert.Null(example3);
            Assert.NotNull(example4);
            Assert.NotNull(example5);
            Assert.True(object.ReferenceEquals(example1, example4));
            Assert.False(object.ReferenceEquals(example1, example5));
            // case sensitive comparison
            Assert.True(example1 == example2.ToLower()); // compare with NEW string
            Assert.True(example1 == example5);
            Assert.False(example1 == example3);
            Assert.True(string.IsNullOrEmpty(example3));
            Assert.True(example1.Equals(example2, StringComparison.OrdinalIgnoreCase));
            // more info: https://docs.microsoft.com/en-gb/dotnet/csharp/how-to/compare-strings
        }

        [Fact]
        public void Delegate_Equality()
        {
            // Act
            Action a, b, c, e, d;

            // Action
            a = () => ActionExecute();
            b = a + a;
            c = a + a;
            e = () => ActionExecute();
            d = null;

            // Assert
            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.NotNull(c);
            Assert.NotNull(e);
            Assert.Null(d);
            Assert.Equal(b, c);
            Assert.True(b == c);
            Assert.False(a == e);
        }

        private void ActionExecute()
        {
        }

    }
}

using DesignPatterns.ChainOfResponsibility;
using Xunit;

namespace DesignPatterns.UnitTests
{
    public class StringMutatorTests
    {
        [Fact]
        public void StringMutatorsTest()
        {
            IStringMutator stringMutator1 = new ToUpperMutator();
            IStringMutator stringMutator2 = new InvertMutator();
            IStringMutator stringMutator3 = new RemoveNumbersMutator();
            IStringMutator stringMutator4 = new TrimMutator();

            stringMutator1
                .SetNext(stringMutator2)
                .SetNext(stringMutator3)
                .SetNext(stringMutator4);

            string actual = stringMutator1.Mutate("    SOME 1 input 2 String 3");

            Assert.Equal("GNIRTS  TUPNI  EMOS", actual);
        }

        [Theory]
        [InlineData("123,456", ",")]
        [InlineData("123.456", ".")]
        [InlineData("some 123number", "some number")]
        [InlineData("123number", "number")]
        [InlineData("text with 01234567890 0987654321 number", "text with   number")]
        [InlineData("number123", "number")]
        public void RemoveNumbersMutatorTest(string input, string expectedResult)
        {
            IStringMutator stringMutator = new RemoveNumbersMutator();

            string actual = stringMutator.Mutate(input);

            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void NullHandlingTest()
        {
            IStringMutator stringMutator = new ToUpperMutator();
            var result = stringMutator.Mutate(null);
            Assert.Null(result);

            stringMutator = new InvertMutator();
            result = stringMutator.Mutate(null);
            Assert.Null(result);

            stringMutator = new RemoveNumbersMutator();
            result = stringMutator.Mutate(null);
            Assert.Null(result);

            stringMutator = new TrimMutator();
            result = stringMutator.Mutate(null);
            Assert.Null(result);
        }

        [Fact]
        public void SingleNextTest()
        {
            IStringMutator sut = new ToUpperMutator();
            IStringMutator stringMutator2 = new InvertMutator();
            IStringMutator stringMutator3 = new RemoveNumbersMutator();
            IStringMutator stringMutator4 = new TrimMutator();

            sut.SetNext(stringMutator2);
            sut.SetNext(stringMutator3);
            sut.SetNext(stringMutator4);

            string actual = sut.Mutate("    some2345Text        ");

            Assert.Equal("SOME2345TEXT", actual);
        }
    }
}
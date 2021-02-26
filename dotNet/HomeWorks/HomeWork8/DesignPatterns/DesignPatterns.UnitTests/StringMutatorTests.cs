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
    }
}
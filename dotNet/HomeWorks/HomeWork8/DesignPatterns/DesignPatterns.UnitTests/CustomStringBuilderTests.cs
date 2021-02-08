using DesignPatterns.Builder;
using Xunit;

namespace DesignPatterns.UnitTests
{
    public class CustomStringBuilderTests
    {
        [Fact]
        public void AppendTest()
        {
            ICustomStringBuilder sb = new CustomStringBuilder();

            sb.Append("The quick brown fox")
                .AppendLine()
                .Append("jumps over the lazy dog")
                .Append('.');

            string text = sb.Build();

            Assert.Equal("The quick brown fox\njumps over the lazy dog.", text);
        }

        [Fact]
        public void EmptyTest()
        {
            ICustomStringBuilder sb = new CustomStringBuilder();

            string text = sb.Build();

            Assert.Equal(string.Empty, text);
        }

        [Fact]
        public void ConstructorTest()
        {
            ICustomStringBuilder sb = new CustomStringBuilder("The quick brown fox jumps over the lazy dog");

            string text = sb.Build();

            Assert.Equal("The quick brown fox jumps over the lazy dog", text);
        }
    }
}

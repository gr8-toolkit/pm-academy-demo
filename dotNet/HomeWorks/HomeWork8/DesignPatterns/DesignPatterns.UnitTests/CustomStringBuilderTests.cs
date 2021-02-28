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
                .AppendLine("jumps over the")
                .Append("lazy")
                .Append(' ')
                .Append("dog")
                .AppendLine('!');

            string text = sb.Build();

            Assert.Equal("The quick brown fox\njumps over the\nlazy dog!\n", text);
        }

        [Fact]
        public void ResultLengthTest()
        {
            ICustomStringBuilder sb = new CustomStringBuilder("Hello! My name is Michael0");
            sb.AppendLine();

            string actual = sb.Build();

            Assert.Equal("Hello! My name is Michael0\n".Length, actual.Length);
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

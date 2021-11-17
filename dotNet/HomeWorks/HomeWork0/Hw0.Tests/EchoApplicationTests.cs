using Hw0.Exercise1;
using Hw0.Tests.Tools;
using System;
using Xunit;

#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace Hw0.Tests
{
    public class EchoApplicationTests
    {
        [Fact]
        public void Run_WithNullArgs_WithoutErrors()
        {
            using var output = ConsoleOutputInterceptor.InterceptOutput();

            var echoApp = new EchoApplication();

            var exitCode = echoApp.Run(null);
            Assert.Equal(0, exitCode);

            var outputStr = output.ToString();
            Assert.Equal(string.Empty, outputStr);
        }

        [Fact]
        public void Run_WithEmptyArgs_WithoutErrors()
        {
            using var output = ConsoleOutputInterceptor.InterceptOutput();

            var echoApp = new EchoApplication();

            var exitCode = echoApp.Run(Array.Empty<string>());
            Assert.Equal(0, exitCode);

            var outputStr = output.ToString();
            Assert.Equal(string.Empty, outputStr);
        }

        [Theory]
        [InlineData(new[] { "hello" }, "hello")]
        [InlineData(new[] { "hello", "world" }, "hello world")]
        [InlineData(new[] { "hello", "world", "WITH", "digits", "1", "2", "3" }, "hello world WITH digits 1 2 3")]
        public void Run_WithValidArgs_WithoutErrors(string[] args, string expected)
        {
            using var output = ConsoleOutputInterceptor.InterceptOutput();

            var echoApp = new EchoApplication();

            var exitCode = echoApp.Run(args);
            Assert.Equal(0, exitCode);

            var outputStr = output.ToString().NormalizeOutput();
            Assert.Equal(expected, outputStr);
        }

        [Theory]
        [InlineData(new[] { null, "" }, " ")]
        [InlineData(new[] { "hello", "", "world" }, "hello  world")]
        [InlineData(new[] { "hello", null, null, "world", null }, "hello   world ")]
        [InlineData(new[] { "hello", "\n", "world", null }, "hello \n world ")]

        public void Run_WithCorruptedArgs_WithoutErrors(string[] args, string expected)
        {
            using var output = ConsoleOutputInterceptor.InterceptOutput();

            var echoApp = new EchoApplication();

            var exitCode = echoApp.Run(args);
            Assert.Equal(0, exitCode);

            var outputStr = output.ToString().NormalizeOutput();
            Assert.Equal(expected, outputStr);
        }
    }
}
#pragma warning restore CA1707 // Identifiers should not contain underscores

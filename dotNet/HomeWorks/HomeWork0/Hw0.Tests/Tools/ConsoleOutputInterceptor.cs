using System;
using System.IO;

namespace Hw0.Tests.Tools
{
    /// <summary>
    /// Console output interceptor for unit tests.
    /// </summary>
    internal class ConsoleOutputInterceptor : IDisposable
    {
        private readonly TextWriter _exWriter;

        public TextWriter Writer { get; }

        private ConsoleOutputInterceptor()
        {
            _exWriter = Console.Out;
            Writer = new StringWriter();
            Console.SetOut(Writer);
        }

        public override string ToString()
        {
            return Writer.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_exWriter);
            Writer.Dispose();
        }

        public static ConsoleOutputInterceptor InterceptOutput()
        {
            return new ConsoleOutputInterceptor();
        }
    }
}

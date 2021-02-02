using System;

namespace Solid.Aim.Services.Impl
{
    internal class Logger : ILogger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
}

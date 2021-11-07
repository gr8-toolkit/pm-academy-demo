namespace Hw0.Exercise1
{
    /// <summary>
    /// Demo exercise.
    /// To run unit test :
    /// use 'dotnet test' CLI command https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test
    /// -or-
    /// use 'Test Explorer' window in MS Visual Studio
    /// -or-
    /// use 'Test' menu in JB Rided
    /// </summary>
    class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        static int Main(string[] args)
        {
            var app = new EchoApplication();
            return app.Run(args);
        }
    }
}
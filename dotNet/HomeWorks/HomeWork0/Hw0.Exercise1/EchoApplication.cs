using System;

namespace Hw0.Exercise1
{
    /// <summary>
    /// Echo application core.
    /// </summary>
    public class EchoApplication
    {
        /// <summary>
        /// Runs echo application.
        /// Prints <paramref name="args"/> to the output.
        /// Arguments will be joined with the white spaces. 
        /// </summary>
        /// <param name="args">Echo arguments.</param>
        /// <returns>Return <c>0</c> in case of success.</returns>
        public int Run(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                var echo = string.Join(' ', args);
                Console.Write(echo);
            }
            return 0;
        }
    }
}

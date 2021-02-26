using System;

namespace RequestProcessor.App.Logging
{
    /// <summary>
    /// Logger abstraction.
    /// </summary>
    internal interface ILogger
    {
        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="message">
        /// Message to log. Empty and null messages should be ignored.
        /// </param>
        void Log(string message);

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="exception">
        /// Exception to log. Empty and null exceptions should be ignored.
        /// </param>
        /// <param name="message">
        /// Optional message to log. 
        /// </param>
        void Log(Exception exception, string message);
    }
}

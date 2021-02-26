using System;

namespace RequestProcessor.App.Exceptions
{
    /// <summary>
    /// Common performer exception.
    /// </summary>
    [Serializable]
    internal class PerformException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PerformException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception details.</param>
        public PerformException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception details.</param>
        /// <param name="innerException">Inner exception.</param>
        public PerformException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

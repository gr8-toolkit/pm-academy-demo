using System;

namespace Solid.Aim.Exceptions
{
    [Serializable]
    internal class AuthException : Exception
    {
        public AuthException()
        {
        }

        public AuthException(string message) 
            : base(message)
        {
        }

        public AuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

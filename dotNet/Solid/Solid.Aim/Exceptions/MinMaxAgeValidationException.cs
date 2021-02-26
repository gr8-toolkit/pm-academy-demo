using System;

namespace Solid.Aim.Exceptions
{
    // LSP demo 
    [Serializable]
    internal class MinMaxAgeValidationException : ValidationException
    {
        public MinMaxAgeValidationException()
        {
        }

        public MinMaxAgeValidationException(string message) 
            : base(message)
        {
        }

        public MinMaxAgeValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

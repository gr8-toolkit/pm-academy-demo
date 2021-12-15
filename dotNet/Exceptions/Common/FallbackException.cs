using System;

namespace Common
{
    public class FallbackException : Exception
    {
        public object InternalData { get; }

        FallbackException(string message) : base(message)
        {
            InternalData = null;
        }

        public FallbackException(object internalData) :
            this($"Can't complete the requested operation, but there are a partial operation results", internalData)
        { }

        public FallbackException(string message, object innerData) : base(message)
        {
            InternalData = innerData;
        }
    }
}

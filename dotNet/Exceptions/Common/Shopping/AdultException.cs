using System;

namespace Common
{
    public class AdultException : ArgumentException
    {
        public int MinAge { get; }

        public AdultException(int innerData) :
            this($"Not allowed adult action for persons aged less  {innerData}", innerData)
        { }

        public AdultException(string message, int minAge) : base(message)
        {
            MinAge = minAge;
        }

    }
}

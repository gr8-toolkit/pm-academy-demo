using System;

namespace DesignPatterns.IoC
{
    public class BrokenType
    {
        public BrokenType()
        {
            throw new ArgumentNullException();
        }
    }
}
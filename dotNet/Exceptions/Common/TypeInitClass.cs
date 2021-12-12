using System;

namespace Common
{
    public class TypeInitClass
    {
        static TypeInitClass()
        {
            throw new Exception("Throws exception in the static constructor");
        }
    }
}

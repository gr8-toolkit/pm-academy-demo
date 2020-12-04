using System;
using System.Collections.Generic;
using System.Text;

namespace OperatorOverloadingExample.Custom
{
    public readonly struct F150
    {
        public readonly int Power;

        public F150(int power)
        {
            Power = power;
        }

        public static F150 operator +(F150 x, F150 y) => new F150(x.Power + y.Power);
        public static F150 operator ++(F150 x) => new F150(x.Power + 1);
        public static F150 operator -(F150 x, F150 y) => throw new InvalidOperationException();
        public static F150 operator *(F150 x, F150 y) => throw new InvalidOperationException();
        public static F150 operator /(F150 x, F150 y) => throw new InvalidOperationException();
        /*The comparison operators must be overloaded in pairs.*/
        public static bool operator <(F150 x, F150 y) => x.Power < y.Power;
        public static bool operator >(F150 x, F150 y) => x.Power > y.Power;
        public static bool operator <=(F150 x, F150 y) => x.Power <= y.Power;
        public static bool operator >=(F150 x, F150 y) => x.Power >= y.Power;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enums
{
    public enum DayInt32Pow2 : short
    {
        Undefined = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
        Weekends = Saturday | Sunday,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday
    }
}

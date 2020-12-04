using System;
using System.Collections.Generic;
using System.Text;

namespace EnumFlagExample.Custom
{
    [Flags]
    public enum DayFlagInt32
    {
        Undefined = 0,                      // 0 = 0000 0000
        Monday = 1,                         // 1 = 0000 0001
        Tuesday = Monday << 1,              // 2 = 0000 0010
        Wednesday = Monday << 2,            // 4 = 0000 0100
        Thursday = Monday << 3,             // 8 = 0000 1000
        Friday = Monday << 4,               // 16 = 0001 0000
        Saturday = Monday << 5,             // 32 = 0010 0000
        Sunday = Monday << 6                // 64 = 0100 0000
    }
}

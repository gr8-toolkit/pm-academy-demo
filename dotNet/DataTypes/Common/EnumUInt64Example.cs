using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum EnumUInt64Example : ulong
    {
        Unknown,                    // 0
        Value1,                     // 1
        Value2,                     // 2
        Value3,                     // 3
        Max = ulong.MaxValue        // 18446744073709551615
    }
}

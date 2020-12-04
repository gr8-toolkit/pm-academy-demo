using System;
using System.Collections.Generic;
using System.Text;

namespace OperatorOverloadingExample.Custom
{
    public class Mustang
    {
        public readonly int Power;

        public Mustang(int power)
        {
            Power = power;
        }

        public static bool operator true(Mustang x) => true;
        public static bool operator false(Mustang x) => false;
        public static Mustang operator &(Mustang x, Mustang y) => new Mustang(x.Power & y.Power);
        public static Mustang operator <<(Mustang x, int y) => new Mustang(x.Power << y);
        public static Mustang operator >>(Mustang x, int y) => new Mustang(x.Power >> y);
        /*The comparison operators must be overloaded in pairs.*/
        public static bool operator ==(Mustang x, Mustang y) => x.Power == y.Power;
        public static bool operator !=(Mustang x, Mustang y) => x.Power == y.Power;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public readonly struct Color
    {
        public static Color Black = new Color(0, 0, 0);
        public static Color White = new Color(255, 255, 255);
        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);

        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}

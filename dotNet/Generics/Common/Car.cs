using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Car
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }

        public Car(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}

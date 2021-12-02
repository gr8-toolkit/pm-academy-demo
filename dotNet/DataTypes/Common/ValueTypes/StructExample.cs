using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ValueTypes
{
    public interface IStructExample
    {
        void Print();
    }

    public readonly struct StructExample : IStructExample
    {
        public readonly uint Param1;
        public readonly byte Param2;
        public readonly bool Param3;
        public readonly int Param4;

        public StructExample(uint p1, byte p2, bool p3, int p4)
        {
            Param1 = p1;
            Param2 = p2;
            Param3 = p3;
            Param4 = p4;
        }

        public void Print()
        {
            Console.WriteLine($"P1: {Param1}{Environment.NewLine}P2: {Param2}{Environment.NewLine}P3: {Param3}");
        }
    }
}

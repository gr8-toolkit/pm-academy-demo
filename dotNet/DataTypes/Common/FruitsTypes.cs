using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Flags]
    public enum FruitsTypes
    {
        None = 0, // required for flags https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=net-6.0
        Empty = 1,
        Apple = 2,
        Avocado = 4,
        Banana = 8,
        Blackberries = 16,
        Cherries = 32,
        Coconut = 64,
        Grapefruit = 128,
        Lemon = 256,
        Mango = 512,
        Orange = 1024,
        Papaya = 2048,
        Peaches = 4096,
        Pineapple = 8192,
        Strawberries = 16384,
    }


    public class FruitsStoreExample
    {
        private FruitsTypes _basket;

        public FruitsTypes Basket => _basket;

        public FruitsStoreExample()
        {
            _basket = FruitsTypes.Empty;
        }

        public void Add(FruitsTypes fruits)
        {
            _basket |= fruits;
        }

        public void Extract(FruitsTypes fruits)
        {
            _basket &= fruits;
        }

        public bool Check(FruitsTypes fruits)
        {
            return (_basket & fruits) != FruitsTypes.None;
        }

        public override string ToString()
        {
            return _basket.ToString();
        }
    }
}

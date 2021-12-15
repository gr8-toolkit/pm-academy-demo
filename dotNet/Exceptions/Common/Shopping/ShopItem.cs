using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ShopItem : Asset
    {
        public Audiences AllowedAudiences { get; set; }

        public int Count { get; set; }
    }
}

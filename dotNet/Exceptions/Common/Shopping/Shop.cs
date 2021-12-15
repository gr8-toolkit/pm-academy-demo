using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class Shop
    {
        private readonly string _name;
        private List<ShopItem> _shopItems;


        public string Name { get => _name; }

        public List<ShopItem> ShopItems { get => _shopItems; }


        public Shop(string name)
        {
            _name = name;
            _shopItems = new List<ShopItem>();
        }

        public void AddItems(IEnumerable<ShopItem> items)
        {
            _shopItems.AddRange(items);
        }

        public bool TrySellItem(string itemName)
        {
            var item = _shopItems.Where(v => v.Name == itemName && v.Count > 0).FirstOrDefault();

            if (item == null)
            {
                return false;
            }

            item.Count--;
            return true;
        }

        public int GetMinItemAge(ShopItem item)
        {
            return item.AllowedAudiences switch
            {
                Audiences.G => 1,
                Audiences.PG => 7,
                Audiences.PG13 => 13,
                Audiences.R => 17,
                Audiences.NC17 => 18,
                _ => int.MaxValue,
            };
        }

        public static List<ShopItem> GenerateDemo()
        {
            return new List<ShopItem>
            {
                new ShopItem
                {
                    Name = "chips",
                    Price = 12.5M,
                    AllowedAudiences = Audiences.PG13,
                    Count = 120
                },
                new ShopItem
                {
                     Name = "wine",
                      Price = 56.5M,
                      AllowedAudiences = Audiences.NC17,
                      Count = 22
                },
                new ShopItem
                {
                    Name = "toy",
                    Price = 6.1M,
                    AllowedAudiences    = Audiences.G,
                    Count = 9
                }
            };
        }
    }
}

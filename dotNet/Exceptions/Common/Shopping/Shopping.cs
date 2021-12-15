using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Shopping
    {
        private readonly Shop _shop;
        private readonly Account _account;
        private readonly List<Asset> _chart;
        private decimal _amount;

        public Shopping(Shop shop, Account account)
        {
            _shop = shop;
            _account = account;
            _amount = 0M;
            _chart = new List<Asset>();
        }
        public bool TryAddToChart(string assetName)
        {
            var item = _shop.ShopItems.Where(v => v.Name == assetName && v.Count > 0).FirstOrDefault();

            bool canBuyItem = item != null;
            canBuyItem &= _shop.TrySellItem(assetName);
            canBuyItem &= TryCheckMinAge(item);

            if (canBuyItem)
            {
                _amount += item.Price;
                _chart.Add(item);
                return true;
            }

            return canBuyItem;
        }

        public bool TryCheckout(out List<Asset> chart)
        {
            if (_amount <= _account.Balance)
            {
                _account.Withdraw(_amount);
                chart = _chart;
                return true;
            }

            chart = Array.Empty<Asset>().ToList();
            return false;
        }

        public void Exit()
        {
            _chart.Clear();
        }

        private bool TryCheckMinAge(ShopItem item)
        {
            var minAge = _shop.GetMinItemAge(item);

            return _account.Age > minAge;
        }

        private void EnsureMinAge(ShopItem item)
        {
            var minAge = _shop.GetMinItemAge(item);

            if (_account.Age <= minAge)
            {
                throw new AdultException(minAge);
            }
        }

    }
}

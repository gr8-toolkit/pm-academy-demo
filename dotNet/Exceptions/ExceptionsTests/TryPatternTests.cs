using Common;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExceptionsTests
{
    public class TryPatternTests
    {
        [Fact]
        public void Parse_String()
        {
            // Act
            string example = "234g";
            int result = default;
            bool success = false;

            // Action

            // Assert
            Assert.Throws<FormatException>(() =>
            {
                result = int.Parse(example);
                success = result != default;
            });
            Assert.False(success);
        }

        [Fact]
        public void TryParse_String()
        {
            // Act
            string example = default;
            int result = default;
            bool success = false;

            // Action
            success = int.TryParse(example, out result);

            // Assert
            Assert.Equal(default, result);
            Assert.False(success);
        }


        [Fact]
        public void TryHandle_Worker()
        {
            // Act
            SomeWorker worker;
            string result;
            bool success;

            // Action
            worker = new SomeWorker();
            success = worker.TryExecute(out result);

            // Assert
            Assert.NotNull(result);
            Assert.False(success);
        }

        [Fact]
        public void Try_Buy_Chips()
        {
            // Act
            var atbShop = new Shop("ATB");
            Func<List<ShopItem>> prepareDemo = () => Shop.GenerateDemo();
            var john15Account = new Account("John", 15, 123.54M);
            var shopping = new Shopping(atbShop, john15Account);
            var buyChips = false;
            var checkout = false;

            // Action
            atbShop.AddItems(prepareDemo());
            buyChips = shopping.TryAddToChart("chips");
            checkout = shopping.TryCheckout(out List<Asset> chart);
            shopping.Exit();

            // Assert
            Assert.True(buyChips);
            Assert.True(checkout);
            Assert.NotNull(chart);
        }


    }
}

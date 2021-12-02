using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataTypesTests.ReferenceTypes
{
    /// <summary>
    /// Strings in action.
    /// </summary>
    public class StringsOperationsTests
    {
        private const string Hell0Str = "hell0";
        private const string HelloStr = "hello";
        private const string WorldStr = "world";

        private static readonly string _helloWorldStr = $"{HelloStr} {WorldStr}";

        [Fact]
        public void Parse_Strings()
        {
            // Act
            var numberString = "112356";
            var otherString = "156Zdo1";
            var dateString = "2020-04-06";
            var maxEnumString = "Max";

            // Action
            var parsedNumber = int.Parse(numberString);
            var parsedDate = DateTime.Parse(dateString);
            var parsedMaxI32Obj = Enum.Parse(typeof(EnumInt32Example), maxEnumString);
            var parsedMaxI32Val = (EnumInt32Example)Enum.Parse(typeof(EnumInt32Example), maxEnumString);
            var parsedMaxI32Int = (int)Enum.Parse(typeof(EnumInt32Example), maxEnumString);
            var parsedMaxUl64Obj = Enum.Parse(typeof(EnumUInt64Example), maxEnumString);
            var parsedMaxUl64Val = (EnumUInt64Example)Enum.Parse(typeof(EnumUInt64Example), maxEnumString);
            var parsedMaxUl64Int = (ulong)Enum.Parse(typeof(EnumUInt64Example), maxEnumString);

            // Assert
            Assert.Equal(112356, parsedNumber);
            Assert.Throws<FormatException>(() => int.Parse(otherString));
            Assert.Equal(new DateTime(2020, 4, 6), parsedDate);
            Assert.NotNull(parsedMaxI32Obj);
            Assert.Equal(EnumInt32Example.Max, parsedMaxI32Val);
            Assert.Equal(int.MaxValue, parsedMaxI32Int);

            Assert.NotNull(parsedMaxUl64Obj);
            Assert.Equal(EnumUInt64Example.Max, parsedMaxUl64Val);
            Assert.Equal(ulong.MaxValue, parsedMaxUl64Int);
        }

        [Fact]
        public void Compare_Strings()
        {
            // Act
            string example1 = Hell0Str;
            string example2 = example1;
            string example3 = Hell0Str;
            string example4 = example3;

            // Action
            bool resul12 = example1 == example2;
            example3 = _helloWorldStr;
            example4 = HelloStr;
            example4 += $" {WorldStr}";

            // Assert 
            Assert.Equal(Hell0Str, example1);
            Assert.Equal(Hell0Str, example2);
            Assert.True(resul12);
            Assert.Equal(_helloWorldStr, example3);
            Assert.Equal(_helloWorldStr, example4);
        }
    }
}

using System;
using Xunit;

namespace DataTypesTests.ValueTypes
{
    public class OtherConvertionTest
    {
        [Fact]
        public void ByteConvertion_Unchecked_Checked()
        {
            // Act
            byte byteE1 = 225;
            sbyte sbyteE1 = default;

            // Action
            unchecked
            {
                // suppress overflow-checking for integral-type
                // arithmetic operations and conversions.
                // default behavior.
                sbyteE1 = (sbyte)byteE1;
            }

            // Assert
            checked
            {
                // enable overflow checking for integral-type
                // arithmetic operations and conversions.
                Assert.Throws<OverflowException>(() => (sbyte)byteE1);
            }
            Assert.Equal(-31, sbyteE1);
        }

        [Fact]
        public void Different_Convertion_Behavior()
        {
            // Act
            var uint64E1 = ulong.MaxValue - 123; // UInt64 = 18446744073709551492

            // Action
            var int32OverflowE1M1 = (int)uint64E1; // Int32 overflow = -124

            // Assert
            Assert.Equal(-124, int32OverflowE1M1);
            Assert.Throws<OverflowException>(() =>
            {
                // throws OverflowException
                // Value was either too large or too small for an Int32.
                var int32OverflowE1M2 = Convert.ToInt32(uint64E1);
            });
        }

        [Fact]
        public void FloatingPoint_To_Integer_Conversion()
        {
            // Act
            var example = -2.224;

            // Action
            var int32E2M1 = Convert.ChangeType(example, typeof(int));              // object
            var int32E2M2 = (int)Convert.ChangeType(example, typeof(int));         // System.Int32 = -2
            var int32E2M3 = Convert.ToInt32(example);                              // System.Int32 = -2

            // Assert
            Assert.NotNull(int32E2M1);
            Assert.Equal(-2, int32E2M2);
            Assert.Equal(-2, int32E2M3);
        }

        [Fact]
        public void Scientific_Notation()
        {
            // Act
            var int32E2M4 = (int)3.414e3;                                           // 3414 = 3.414 * (10^3), M=3.414, n=10, p=3
            var int32E2M5 = (int)445000E-2;                                         // 4450 =445000 * (10^-2), M=445000, n=10, p=-2

            // Action

            // Assert
            Assert.Equal(3414, int32E2M4); // 3414 = 3.414 * (10^3), M=3.414, n=10, p=3
            Assert.Equal(4450, int32E2M5); // 4450 =445000 * (10^-2), M=445000, n=10, p=-2
        }
    }
}

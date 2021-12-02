using Xunit;

namespace DataTypesTests.ValueTypes
{
    /// <summary>
    /// Implisit convertion examples.
    /// No implicit conversion for Bool.
    /// No implicit conversion for Double.
    /// No implicit conversion for Decimal.
    /// </summary>
    public class ImplicitConvertionTest
    {

        [Fact]
        public void Byte_Convertion()
        {
            // Act
            byte byteE1 = 112;

            // Action
            short shortE1 = byteE1;
            ushort ushortE1 = byteE1;
            int intE1 = byteE1;
            uint uintE1 = byteE1;
            long longE1 = byteE1;
            ulong ulongE1 = byteE1;
            float floatE1 = byteE1;
            double doubleE1 = byteE1;
            decimal decimalE1 = byteE1;

            // Assert
            Assert.Equal(112, shortE1);
            Assert.Equal(112, ushortE1);
            Assert.Equal(112, intE1);
            Assert.Equal(112U, uintE1);
            Assert.Equal(112, longE1);
            Assert.Equal(112UL, ulongE1);
            Assert.Equal(112F, floatE1);
            Assert.Equal(112, doubleE1);
            Assert.Equal(112M, decimalE1);
        }

        [Fact]
        public void Short_Convertion()
        {
            // Act
            short shortE1 = 11211;

            // Action
            int intE1 = shortE1;
            long longE1 = shortE1;
            float floatE1 = shortE1;
            double doubleE1 = shortE1;
            decimal decimalE1 = shortE1;

            // Assert
            Assert.Equal(11211, intE1);
            Assert.Equal(11211, longE1);
            Assert.Equal(11211, floatE1);
            Assert.Equal(11211, doubleE1);
            Assert.Equal(11211, decimalE1);
        }

        [Fact]
        public void Int_Convertion()
        {
            // Act
            int intE1 = 1221231331;

            // Action
            long longE1 = intE1;
            float floatE1 = intE1;
            double doubleE1 = intE1;
            decimal decimalE1 = intE1;

            // Assert
            Assert.Equal(1221231331, longE1);
            Assert.Equal(1221231331, floatE1);
            Assert.Equal(1221231331, doubleE1);
            Assert.Equal(1221231331, decimalE1);
        }

        [Fact]
        public void Long_Convertion()
        {
            // Act
            long longE1 = 12212311122331;

            // Action
            float floatE1 = longE1;
            double doubleE1 = longE1;
            decimal decimalE1 = longE1;

            // Assert
            Assert.Equal(12212311122331, floatE1);
            Assert.Equal(12212311122331, doubleE1);
            Assert.Equal(12212311122331, decimalE1);
        }

        [Fact]
        public void Char_Convertion()
        {
            // Act
            char charE1 = 'O';

            // Action
            ushort ushortE1 = charE1;
            int intE1 = charE1;
            uint uintE1 = charE1;
            long longE1 = charE1;
            ulong ulongE1 = charE1;
            float floatE1 = charE1;
            double doubleE1 = charE1;
            decimal decimalE1 = charE1;

            // Assert
            Assert.Equal(79, ushortE1);
            Assert.Equal(79, intE1);
            Assert.Equal(79U, uintE1);
            Assert.Equal(79, longE1);
            Assert.Equal(79UL, ulongE1);
            Assert.Equal(79F, floatE1);
            Assert.Equal(79, doubleE1);
            Assert.Equal(79M, decimalE1);
        }

        [Fact]
        public void Float_Convertion()
        {
            // Act
            float floatE1 = 11212.2f;

            // Action
            double doubleE1 = floatE1;

            // Assert
            Assert.Equal(11212.2001953125, doubleE1);
        }
    }
}

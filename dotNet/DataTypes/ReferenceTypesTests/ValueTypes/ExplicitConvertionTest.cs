using Xunit;

namespace DataTypesTests.ValueTypes
{
    /// <summary>
    /// Explicit convertion examples.
    /// </summary>
    public class ExplicitConvertionTest
    {
        [Fact]
        public void Byte_Convertion()
        {
            // Act
            byte byteE1 = 128;

            // Action
            sbyte sbyteE1 = (sbyte)byteE1;
            char charE1 = (char)byteE1;

            // Assert
            Assert.Equal(-128, sbyteE1); // sbyte.Max = 127
            Assert.Equal('\u0080', charE1);
        }

        [Fact]
        public void Short_Convertion()
        {
            // Act
            short shortE1 = 21221;

            // Action
            sbyte sbyteE1 = (sbyte)shortE1;
            byte byteE1 = (byte)shortE1;
            ushort ushortE1 = (ushort)shortE1;
            uint uintE1 = (uint)shortE1;
            ulong ulongE1 = (ulong)shortE1;
            char charE1 = (char)shortE1;

            // Assert
            Assert.Equal(-27, sbyteE1);
            Assert.Equal(229, byteE1);
            Assert.Equal(21221, ushortE1);
            Assert.Equal(21221U, uintE1);
            Assert.Equal(21221UL, ulongE1);
            Assert.Equal('勥', charE1);
        }

        [Fact]
        public void Int_Convertion()
        {
            // Act
            int intE1 = 123421212;

            // Action
            sbyte sbyteE1 = (sbyte)intE1;
            byte byteE1 = (byte)intE1;
            short shortE1 = (short)intE1;
            ushort ushortE1 = (ushort)intE1;
            uint uintE1 = (uint)intE1;
            ulong ulongE1 = (ulong)intE1;
            char charE1 = (char)intE1;

            // Assert
            Assert.Equal(28, sbyteE1);
            Assert.Equal(28, byteE1);
            Assert.Equal(16924, shortE1);
            Assert.Equal(16924, ushortE1);
            Assert.Equal(123421212U, uintE1);
            Assert.Equal(123421212UL, ulongE1);
            Assert.Equal('䈜', charE1);
        }

        [Fact]
        public void Long_Convertion()
        {
            // Act
            long longE1 = 1234211125761212;

            // Action
            sbyte sbyteE1 = (sbyte)longE1;
            byte byteE1 = (byte)longE1;
            short shortE1 = (short)longE1;
            ushort ushortE1 = (ushort)longE1;
            int intE1 = (int)longE1;
            uint uintE1 = (uint)longE1;
            ulong ulongE1 = (ulong)longE1;
            char charE1 = (char)longE1;

            // Assert
            Assert.Equal(-68, sbyteE1);
            Assert.Equal(188, byteE1);
            Assert.Equal(-27460, shortE1);
            Assert.Equal(38076, ushortE1);
            Assert.Equal(733648060, intE1);
            Assert.Equal(733648060U, uintE1);
            Assert.Equal(1234211125761212UL, ulongE1);
            Assert.Equal('钼', charE1);
        }

        [Fact]
        public void Char_Convertion()
        {
            // Act
            char charE1 = 'Q';

            // Action
            sbyte sbyteE1 = (sbyte)charE1;
            byte byteE1 = (byte)charE1;
            short shortE1 = (short)charE1;

            // Assert
            Assert.Equal(81, sbyteE1);
            Assert.Equal(81, byteE1);
            Assert.Equal(81, shortE1);
        }

        [Fact]
        public void Float_Convertion()
        {
            // Act
            float floatE1 = 1113.34f;

            // Action
            sbyte sbyteE1 = (sbyte)floatE1;
            byte byteE1 = (byte)floatE1;
            short shortE1 = (short)floatE1;
            ushort ushortE1 = (ushort)floatE1;
            int intE1 = (int)floatE1;
            uint uintE1 = (uint)floatE1;
            long longE1 = (long)floatE1;
            ulong ulongE1 = (ulong)floatE1;
            char charE1 = (char)floatE1;
            decimal decimalE1 = (decimal)floatE1;

            // Assert
            Assert.Equal(89, sbyteE1);
            Assert.Equal(89, byteE1);
            Assert.Equal(1113, shortE1);
            Assert.Equal(1113, ushortE1);
            Assert.Equal(1113, intE1);
            Assert.Equal(1113U, uintE1);
            Assert.Equal(1113, longE1);
            Assert.Equal(1113UL, ulongE1);
            Assert.Equal('љ', charE1);
            Assert.Equal(1113.34M, decimalE1);
        }

        [Fact]
        public void Doule_Convertion()
        {
            // Act
            double doubleE1 = 133113.34944f;

            // Action
            sbyte sbyteE1 = (sbyte)doubleE1;
            byte byteE1 = (byte)doubleE1;
            short shortE1 = (short)doubleE1;
            ushort ushortE1 = (ushort)doubleE1;
            int intE1 = (int)doubleE1;
            uint uintE1 = (uint)doubleE1;
            long longE1 = (long)doubleE1;
            ulong ulongE1 = (ulong)doubleE1;
            char charE1 = (char)doubleE1;
            float floatE1 = (float)doubleE1;
            decimal decimalE1 = (decimal)doubleE1;

            // Assert
            Assert.Equal(-7, sbyteE1);
            Assert.Equal(249, byteE1);
            Assert.Equal(2041, shortE1);
            Assert.Equal(2041, ushortE1);
            Assert.Equal(133113, intE1);
            Assert.Equal(133113U, uintE1);
            Assert.Equal(133113, longE1);
            Assert.Equal(133113UL, ulongE1);
            Assert.Equal('߹', charE1);
            Assert.Equal(133113.34375F, floatE1);
            Assert.Equal(133113.34375M, decimalE1);
        }

        [Fact]
        public void DecimalExplicitConvertion()
        {
            // Act
            double decimalE1 = 133112213.32234944f;

            // Action
            sbyte sbyteE1 = (sbyte)decimalE1;
            byte byteE1 = (byte)decimalE1;
            short shortE1 = (short)decimalE1;
            ushort ushortE1 = (ushort)decimalE1;
            int intE1 = (int)decimalE1;
            uint uintE1 = (uint)decimalE1;
            long longE1 = (long)decimalE1;
            ulong ulongE1 = (ulong)decimalE1;
            char charE1 = (char)decimalE1;
            float floatE1 = (float)decimalE1;
            double doubleE1 = (double)decimalE1;

            // Assert
            Assert.Equal(-104, sbyteE1);
            Assert.Equal(152, byteE1);
            Assert.Equal(8600, shortE1);
            Assert.Equal(8600, ushortE1);
            Assert.Equal(133112216, intE1);
            Assert.Equal(133112216U, uintE1);
            Assert.Equal(133112216, longE1);
            Assert.Equal(133112216UL, ulongE1);
            Assert.Equal('↘', charE1);
            Assert.Equal(133112216, floatE1);
            Assert.Equal(133112216, doubleE1);
        }
    }
}

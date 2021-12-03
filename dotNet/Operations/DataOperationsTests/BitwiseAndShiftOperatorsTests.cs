using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataOperationsTests
{
    public class BitwiseAndShiftOperatorsTests
    {
        [Fact]
        public void Bitwise()
        {
            // Act
            int z1, z2, z3, z4, z5, z6;
            uint a, b, c1, c2;

            // Action
            z1 = 25;                    // 0000 0000 0001 1001 = 25
            z2 = 72;                    // 0000 0000 0100 1000 = 72
            z3 = z1 & z2;               // 0000 0000 0000 1000 = 8
            z4 = z1 | z2;               // 0000 0000 0101 1001 = 89
            z5 = z1 ^ z2;               // 0000 0000 0101 0001 = 81
            z6 = ~z1;                   // 11111111111111111111111111111111111111111111111111111111 1110 0110 = -26

            a = 0b_1010_0000;      // 1010 0000 = 160
            b = 0b_1001_0001;      // 1001 0001 = 145
            c1 = a | b;            // 1011 0001 = 177
            c2 = 0b_1011_0001;     // 177

            // Assert
            Assert.Equal(8, z3);
            Assert.Equal(89, z4);
            Assert.Equal(81, z5);
            Assert.Equal(-26, z6);

            Assert.Equal(177U, c1);
            Assert.Equal(177U, c2);
            Assert.True(c1 == c2);
        }

        [Fact]
        public void Shift()
        {
            // Act
            int s1, s2, s3;

            // Action
            s1 = 10;                    //  0000 0000 0000 1010
            s2 = s1 >> 1;               //  0000 0000 0000 0101
            s3 = s1 << 1;               //  0000 0000 0001 0100

            // Assert
            Assert.Equal(5, s2);
            Assert.Equal(20, s3);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataOperationsTests
{
    public class ArithmeticOperatorsTests
    {
        [Fact]
        public void Unary()
        {
            // Act
            int a, b, c, d, e, f, g, h;
            char i, j, k;

            // Action
            a = 441;
            b = a;          // b = 441
            c = a++;        // a = 442, c = 441
            d = c;          // d = 441
            e = d;          // e = 441
            f = ++e;        // e = 442, f = 442
            g = -f;         // g = -442
            h = -g;         // h = 442
            i = 'b';
            j = i;
            k = ++j;        // j='c', k='c'

            // Assert
            Assert.Equal(442, a);
            Assert.Equal(441, b);
            Assert.Equal(441, c);
            Assert.Equal(441, d);
            Assert.Equal(442, e);
            Assert.Equal(442, f);
            Assert.Equal(-442, g);
            Assert.Equal(442, h);
            Assert.Equal('c', k);
            Assert.Equal('c', j);
        }

        [Fact]
        public void Binary()
        {
            // Act
            int a, b, c, d, e, f, g, h, l;
            float i, j;

            // Action
            a = 98;
            b = a + 2;
            c = b * 2;
            d = a / 2;
            e = a % 2;
            f = a % 3;
            g = a % -3;
            h = a / 3;                              // integer number
            i = (float)a / -3;
            j = (float)a / 3;                       // real number
            l = Math.DivRem(a, 3, out int m);

            // Assert
            Assert.Equal(98, a);
            Assert.Equal(100, b);
            Assert.Equal(200, c);
            Assert.Equal(49, d);
            Assert.Equal(0, e);
            Assert.Equal(2, f);
            Assert.Equal(2, g);
            Assert.Equal(32, h);
            Assert.Equal(32, l);
            Assert.Equal(2, m);
            Assert.Equal(-32.666668f, i);
            Assert.Equal(32.666668f, j);
        }


        [Fact]
        public void Other()
        {
            // Act
            int a, b, c, d, e, f, g, h;

            // Action
            a = 4;
            b = 5;
            c = 3;
            d = a * b - c;
            e = a * 4 - c * 2;
            f = a * (b - c);
            g = 2 * (2 - 3);
            h = 1 + (2 * (6 - c)) * (b - c) + 2;

            // Assert
            Assert.Equal(17, d);
            Assert.Equal(10, e);
            Assert.Equal(8, f);
            Assert.Equal(-2, g);
            Assert.Equal(15, h);
        }
    }
}

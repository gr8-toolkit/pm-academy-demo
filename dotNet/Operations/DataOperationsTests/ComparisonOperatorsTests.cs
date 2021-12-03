using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataOperationsTests
{
    public class ComparisonOperatorsTests
    {
        [Fact]
        public void Simple_Comparison()
        {
            // Act
            int n1 = 5, n2 = 6, n3 = 5;
            bool a1, a2, a3, a4, b1, b2, b3, b4;

            // Action
            a1 = n1 == n2;
            a2 = n2 != n3;
            a3 = n1 < n2;
            a4 = n2 >= n1;
            b1 = a1 && a2;
            b2 = n1 == n2 && n2 != n3;
            b3 = a1 || a2;
            b4 = !a1;

            // Assert
            Assert.False(a1);
            Assert.True(a2);
            Assert.True(a3);
            Assert.True(a4);

            Assert.False(b1);
            Assert.False(b2);
            Assert.True(b3);
            Assert.True(b4);
        }

        [Fact]
        public void NullCoalesing()
        {
            // Act

            string[] stringsArrray = new[] { "a", null, "c", "d" };
            string nullVal, notNullVal1, notNullVal2, notNullVa3;
            int? nullNumb;
            char? nullChar1, nullChar2;

            // Action
            nullVal = stringsArrray[1];                                     // null
            notNullVal1 = stringsArrray[2];                                 // "c"
            notNullVal2 = nullVal ?? "b";
            notNullVa3 = string.IsNullOrEmpty(notNullVal1) ? "s" : "q";     // "q"
            nullNumb = nullVal?.Length;                                     // null
            nullChar1 = notNullVal1?[0];                                    // 'c'
            nullChar2 = nullVal?[0];                                         // null

            // Assert
            Assert.NotNull(stringsArrray);
            Assert.Null(nullVal);
            Assert.NotNull(notNullVal1);
            Assert.Equal("b", notNullVal2);
            Assert.Equal("q", notNullVa3);
            Assert.Null(nullNumb);
            Assert.Equal('c', nullChar1);
            Assert.Null(nullChar2);

            Assert.Throws<NullReferenceException>(() => { char ch = nullVal[0]; });
            Assert.Throws<NullReferenceException>(() => { int numb = nullVal.Length; });
        }
    }
}

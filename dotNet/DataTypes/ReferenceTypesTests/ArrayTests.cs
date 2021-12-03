using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataTypesTests
{
    public class ArrayTests
    {
        [Fact]
        public void Inline_Array_Initialization()
        {
            // Act
            int[] numericArrayE1 = new int[3];                                      // 0,0,0
            int[] numericArrayE2 = new int[] { 5, 7, 2 };                           // 5,7,2
            int[] numericArrayE3 = null;
            int[] numericArrayE4 = null;

            // Action
            numericArrayE3 = new[] { 1, 2, 2 };                                     // 1,2,2

            // Assert
            Assert.NotNull(numericArrayE1);
            Assert.NotNull(numericArrayE2);
            Assert.NotNull(numericArrayE3);
            Assert.Null(numericArrayE4);

            Assert.Equal(3, numericArrayE1.Length);
            Assert.Equal(3, numericArrayE2.Length);
            Assert.Equal(3, numericArrayE3.Length);
            Assert.All(numericArrayE1, result => Assert.Equal(default, result));
        }

        [Fact]
        public void Inline_Array_Modification()
        {
            // Act
            int[] numericArrayE1 = new int[3];                                      // 0,0,0
            int[] numericArrayE2 = new int[] { 5, 7, 2 };                           // 5,7,2
            int[] numericArrayE3 = new[] { 1, 2, 2 };                               // 1,2,2
            int[] numericArrayE4 = null;

            // Action
            numericArrayE1[1] = 13;                                                 // 0 >> 13
            numericArrayE2[2] = 13;                                                 // 2 >> 13

            // Assert
            Assert.Contains<int>(13, numericArrayE1);
            Assert.DoesNotContain<int>(2, numericArrayE2);
            Assert.Contains<int>(13, numericArrayE2);

            Assert.Throws<IndexOutOfRangeException>(() => numericArrayE1[3] = 3);
            Assert.Throws<NullReferenceException>(() => numericArrayE4.Length);
        }

        [Fact]
        public void Matrix_Array_Initialization()
        {
            // Act
            int[,] numericArrayE1 = new int[2, 3];                                      // 0,0,0 | 0,0,0 
            int[,] numericArrayE2 = null;
            int[,] numericArrayE3 = { { 1, 2, 3 }, { 7, 8, 9 } };                       // 1,2,3 | 7,8,9
            int[,] numericArrayE4 = null;

            // Action
            numericArrayE2 = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };                // 1,2,3 | 4,5,6 

            // Assert
            Assert.NotNull(numericArrayE1);
            Assert.NotNull(numericArrayE2);
            Assert.NotNull(numericArrayE3);
            Assert.Null(numericArrayE4);
        }

        [Fact]
        public void Matrix_Array_Modification()
        {
            // Act
            int[,] numericArrayE1 = new int[2, 3];                                      // 0,0,0 | 0,0,0 
            int[,] numericArrayE2 = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };         // 1,2,3 | 4,5,6 
            int[,] numericArrayE3 = { { 1, 2, 3 }, { 7, 8, 9 } };                       // 1,2,3 | 7,8,9

            int row1 = 0, cell1 = 1;
            int row2 = 1, cell2 = 2;

            // Action
            numericArrayE1[row1, cell1] = 12;                                           // 0 >> 12
            numericArrayE2[row2, cell2] = 45;                                           // 6 >> 45

            // Assert
            Assert.NotNull(numericArrayE1);
            Assert.NotNull(numericArrayE2);
            Assert.NotNull(numericArrayE3);
            Assert.Throws<IndexOutOfRangeException>(() => numericArrayE2[2, 2] = 5);
        }
    }
}

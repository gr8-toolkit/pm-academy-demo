using Common.ReferenceTypes;
using System;
using Xunit;
namespace DataTypesTests
{
    public class BoxingUnboxingTests
    {
        private const int InitialNumber = 10101;

        [Fact]
        public void Implicit_Boxing()
        {
            // Act
            int int1 = InitialNumber;                   // System.Int32
            long long1 = 10101;                         // System.Int64

            // Action
            object boxed1 = int1;                       // boxing a1
            object boxed2 = long1;                      // boxing a2
            object boxed3 = InitialNumber;              // boxing of System.Int32

            // Assert
            Assert.NotNull(boxed1);
            Assert.NotNull(boxed2);
            Assert.NotNull(boxed3);
        }


        [Fact]
        public void Explicit_Unboxing()
        {
            // Act
            object boxed1 = InitialNumber;              // unboxing b1
            object boxed2 = (long)InitialNumber;
            int int1, int2, int3;
            long long1, long2;

            // Action
            int1 = (int)boxed1;
            int3 = (int)(long)boxed2;
            long2 = (long)boxed2;

            // Assert
            Assert.Throws<InvalidCastException>(() =>
            {
                // Unable to cast object of type 'System.Int64' to type 'System.Int32'
                int2 = (int)boxed2;
            });
            Assert.Equal(InitialNumber, int3);
            Assert.Throws<InvalidCastException>(() =>
            {
                // Unable to cast object of type 'System.Int32' to type 'System.Int64'.
                long1 = (long)boxed1;
            });
            Assert.Equal(InitialNumber, long2);

        }
    }
}

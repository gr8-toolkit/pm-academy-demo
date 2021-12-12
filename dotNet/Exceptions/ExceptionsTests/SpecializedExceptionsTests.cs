using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExceptionsTests
{
    public class SpecializedExceptionsTests
    {
        [Fact]
        public void Handle_IndexOutOfRangeException()
        {
            // Act
            int[] numbersArray = { 1, 2, 3, };
            int fourthElement;
            bool iorWorksFlag;

            // Action
            try
            {

                iorWorksFlag = false;
                fourthElement = numbersArray[3];
            }
            catch (IndexOutOfRangeException)
            {
                iorWorksFlag = true;
            }

            // Assert
            Assert.True(iorWorksFlag);
        }


        [Fact]
        public void Handle_DivideByZeroException()
        {
            // Act
            int divisor, divided, result;
            bool dbzWorksFlag;

            // Action
            try
            {
                dbzWorksFlag = false;
                divisor = 0;
                divided = 10;
                result = divided / divisor;
            }
            catch (DivideByZeroException)
            {
                dbzWorksFlag = true;
            }

            // Assert
            Assert.True(dbzWorksFlag);
        }

        [Fact]
        public void Handle_InvalidCastException()
        {
            // Act
            int a, b;
            object c;
            long e;
            bool icWorksFlag, boxingWorksFlag, unboxingInt32Works, unboxingInt64Works;

            // Action

            icWorksFlag = false;
            boxingWorksFlag = false;
            unboxingInt32Works = false;
            unboxingInt64Works = false;
            try
            {
                a = 1;
                c = a;
                boxingWorksFlag = true;
                b = (int)c;
                unboxingInt32Works = true;
                e = (long)c;
                unboxingInt64Works = true;

            }
            catch (InvalidCastException)
            {
                icWorksFlag = true;
            }

            // Assert
            Assert.True(icWorksFlag);
            Assert.True(boxingWorksFlag);
            Assert.True(unboxingInt32Works);
            Assert.False(unboxingInt64Works);
        }

        [Fact]
        public void Handle_NullReferenceException()
        {
            // Act
            string example;
            bool nrWorksFlag;

            // Action
            nrWorksFlag = false;
            example = null;
            try
            {
                var length = example.Length;
            }
            catch (NullReferenceException)
            {
                nrWorksFlag = true;
            }

            // Assert
            Assert.True(nrWorksFlag);
        }

        [Fact]
        public void Handle_TypeInitializationException()
        {
            // Act
            TypeInitClass example;
            bool tiWorksFlag;

            // Action
            tiWorksFlag = false;
            try
            {
                example = new TypeInitClass();
            }
            catch (TypeInitializationException)
            {
                tiWorksFlag = true;
            }

            // Assert
            Assert.True(tiWorksFlag);
        }



    }
}

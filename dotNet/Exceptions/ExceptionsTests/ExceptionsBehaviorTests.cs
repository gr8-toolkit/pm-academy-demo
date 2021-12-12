using Common;
using System;
using Xunit;

namespace ExceptionsTests
{
    public class ExceptionsBehaviorTests
    {
        [Fact]
        public void Code_Throws_UnHandled_Exception()
        {
            // Act
            int divisor, divided, result;

            // Action
            divisor = 0;
            divided = 10;

            // Assert
            Assert.Throws<DivideByZeroException>(() => result = divided / divisor);
            //Assert.Throws<Exception>(() => result = divided / divisor); // false
            Assert.ThrowsAny<Exception>(() => result = divided / divisor);
        }

        [Fact]
        public void Catch_Exception()
        {
            // Act
            int divisor, divided, result;
            bool mainEnterWorksFlag, mainExitWorksFlag, dbzWorksFlag, baseExceptionWorksFlag, finallyWorksFlag;

            // Action
            divisor = 0;
            divided = 10;

            // assign local variables for test purposes only
            mainEnterWorksFlag = false;
            mainExitWorksFlag = false;
            dbzWorksFlag = false;
            baseExceptionWorksFlag = false;
            //finallyWorksFlag = false; // finally block always executed, this code not needed 

            try
            {
                mainEnterWorksFlag = true;
                result = divided / divisor;
                mainExitWorksFlag = true;
            }
            catch (DivideByZeroException dze)
            {
                dbzWorksFlag = true;

            }
            catch (Exception e)
            {
                baseExceptionWorksFlag = true;
            }
            finally
            {
                finallyWorksFlag = true;
            }

            // Assert
            Assert.True(mainEnterWorksFlag);
            Assert.False(mainExitWorksFlag);
            Assert.True(dbzWorksFlag);
            Assert.False(baseExceptionWorksFlag);
            Assert.True(finallyWorksFlag);
        }

        [Fact]
        public void Handle_Exception()
        {
            // Act
            string example;
            bool finallyWorksFlag;

            // Action
            example = null;
            finallyWorksFlag = false;
            try
            {
                var length = example.Length;
            }
            catch (Exception)
            {
            }
            finally
            {
                finallyWorksFlag = true;
            }

            // Assert
            Assert.True(finallyWorksFlag);
        }

        [Fact]
        public void Throw_Exception()
        {
            // Act
            string example, errorMessage;
            bool anExceptionWorksFlag, nrExceptionWorksFlag, baseExceptionWorksFlag;

            // Assert
            example = string.Empty;
            errorMessage = null;
            anExceptionWorksFlag = false;
            nrExceptionWorksFlag = false;
            baseExceptionWorksFlag = false;
            try
            {
                if (string.IsNullOrEmpty(example))
                {
                    throw new ArgumentNullException(nameof(example));
                }
            }
            catch (ArgumentNullException ane)
            {
                anExceptionWorksFlag = true;
                errorMessage = ane.Message;
            }
            catch (NullReferenceException nre)
            {
                nrExceptionWorksFlag = true;
                errorMessage = nre.Message;
            }
            catch (Exception e)
            {
                baseExceptionWorksFlag = true;
                errorMessage = e.Message;
            }

            // Assert
            Assert.True(anExceptionWorksFlag);
            Assert.False(nrExceptionWorksFlag);
            Assert.False(baseExceptionWorksFlag);
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public void Handle_Worker_Exception()
        {
            // Act
            SomeWorker worker;
            bool baseExceptionWorksFlag, finallyWorksFlag;
            string result;

            // Action
            result = null;
            baseExceptionWorksFlag = false;
            worker = new SomeWorker();
            try
            {
                result = worker.Execute();
            }
            catch (Exception)
            {
                baseExceptionWorksFlag = true;
            }
            finally
            {
                finallyWorksFlag = true;
            }

            // Assert
            Assert.True(baseExceptionWorksFlag);
            Assert.True(finallyWorksFlag);
            Assert.Null(result);
        }

        [Fact]
        public void Handle_Worker_Fallback()
        {
            // Act
            SomeWorker worker;
            bool baseExceptionWorksFlag, finallyWorksFlag;
            string result;

            // Action
            result = null;
            baseExceptionWorksFlag = false;
            worker = new SomeWorker();
            try
            {
                worker.ExecuteWithFallback();
            }
            catch (FallbackException fex) when (fex.InternalData != null)
            {
                result = (string)fex.InternalData;
            }
            catch (Exception)
            {
                baseExceptionWorksFlag = true;
            }
            finally
            {
                finallyWorksFlag = true;
            }

            // Assert
            Assert.NotNull(result);
            Assert.False(baseExceptionWorksFlag);
            Assert.True(finallyWorksFlag);
        }

        [Fact]
        public void TryHandle_Worker()
        {
            // Act
            SomeWorker worker;
            string result;
            bool success;

            // Action
            worker = new SomeWorker();
            success = worker.TryExecute(out result);

            // Assert
            Assert.NotNull(result);
            Assert.False(success);
        }

    }


}

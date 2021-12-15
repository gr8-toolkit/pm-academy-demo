using Common;
using System;
using System.Threading;
using System.Threading.Tasks;
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
                result = worker.ExecuteAtomic();
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
                worker.ExecuteDirty();
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
        public async Task Handle_Operation_Cancellation()
        {
            // Act
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)); // cancel task after 3 seconds
            var ct = cts.Token;
            string result = default;
            var tcExceptionWorksFlag = false;
            var ocExceptionWorksFlag = false;
            var baseExceptionWorksFlag = false;

            // Action
            try
            {
                var stuff = new Stuff();
                result = await stuff.SomeExpensiveFunc(ct);
            }
            catch (TaskCanceledException)
            {
                tcExceptionWorksFlag = true;
            }
            catch (OperationCanceledException) // includes TaskCanceledException
            {
                ocExceptionWorksFlag = true;
            }
            catch (Exception)
            {
                baseExceptionWorksFlag = true;
            }

            // Assert
            Assert.Null(result);
            Assert.True(tcExceptionWorksFlag);
            Assert.False(ocExceptionWorksFlag);
            Assert.False(baseExceptionWorksFlag);
        }

        [Fact]
        public async Task Handle_Aggregated_Exception()
        {
            // Act
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3)); // cancel task after 3 seconds
            var ct = cts.Token;
            string result = default;
            var tcExceptionWorksFlag = false;
            var dnfExceptionWorksFlag = false;
            var agExceptionWorksFlag = false;
            var baseExceptionWorksFlag = false;
            DllNotFoundException dllNotFoundException = default;
            TaskCanceledException taskCanceledException = default;

            // Action
            try
            {
                var stuff = new Stuff();
                result = await stuff.SomeUnexcpectedFunc(ct);
            }
            catch (TaskCanceledException)
            {
                tcExceptionWorksFlag = true;
            }
            catch (DllNotFoundException)
            {
                dnfExceptionWorksFlag = true;
            }
            catch (AggregateException ae) when (ae.InnerException != null || ae.InnerExceptions.Count > 0)
            {
                agExceptionWorksFlag = true;

                // let's handle known inner exceptions
                ae.Handle(ex =>
                {
                    if (ex is DllNotFoundException dnfException)
                    {
                        dllNotFoundException = dnfException;
                        return true;
                    }

                    if (ex is TaskCanceledException tcException)
                    {
                        taskCanceledException = tcException;
                        return true;
                    }

                    // re-throw any unhandled exception
                    return false;
                });
            }
            catch (Exception)
            {
                baseExceptionWorksFlag = true;
            }

            // Assert
            Assert.Null(result);
            Assert.NotNull(dllNotFoundException);
            Assert.NotNull(taskCanceledException);
            Assert.False(tcExceptionWorksFlag);
            Assert.True(agExceptionWorksFlag);
            Assert.False(dnfExceptionWorksFlag);
            Assert.False(baseExceptionWorksFlag);
        }

        [Fact]
        public async Task ReThrow_Exception()
        {
            // Act
            string error = "error";
            string result = default;
            bool handleLevel1Flag = false;
            bool handleLevel2Flag = false;


            // Action
            try
            {
                try
                {
                    throw new Exception(error);
                }
                catch (Exception level1)
                {
                    handleLevel1Flag = true;
                    throw; // re-throw current error, keep StackTrace
                    //throw level1; // alsp re-throw current error
                }
            }
            catch (Exception level2)
            {
                handleLevel2Flag = true;
                result = level2.Message;
            }

            // Assert
            Assert.True(handleLevel1Flag);
            Assert.True(handleLevel2Flag);
            Assert.NotNull(result);
            Assert.Equal(error, result);
        }

        [Fact]
        public async Task ReThrow_New_Exception()
        {
            // Act
            string error1 = "error1", error2 = "error2";
            string result = default;
            bool handleLevel1Flag = false, handleLevel2Flag = false;

            // Action
            try
            {
                try
                {
                    throw new Exception(error1);
                }
                catch (Exception level1)
                {
                    handleLevel1Flag = true;
                    throw new Exception(error2); // throw new error, new StackTrace
                }
            }
            catch (Exception level2)
            {
                handleLevel2Flag = true;
                result = level2.Message;
            }

            // Assert
            Assert.True(handleLevel1Flag);
            Assert.True(handleLevel2Flag);
            Assert.NotNull(result);
            Assert.Equal(error2, result);
        }



    }


}

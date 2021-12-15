using System;
using System.Net.Http;

namespace Common
{
    internal class DoAndNotDo
    {
        private void DoNotThrowExplicitly()
        {
            throw new Exception();
            throw new SystemException();
            throw new ApplicationException();
            throw new StackOverflowException();
            throw new OutOfMemoryException();
        }

        private void DoNotCatch()
        {
            try
            {

            }
            catch (Exception ex)
            {
                // AVOID catching Exception or SystemException,
                // except in top-level exception handlers.
                // or handle and re-throw new exception
            }
        }

        private void DoNotThrowInPublicApi()
        {
            throw new NullReferenceException();
            throw new IndexOutOfRangeException();
            throw new AccessViolationException();
        }

        // Design classes so that exceptions can be avoided
        private void AvoidExceptions()
        {
            var array = new int[] { 1, 2, 4, 5, 6 };
            for (var i = 0; i < array.Length; i++)
            {
                // avoid IndexOutOfRangeException 
                array[i] = 1;
            }
        }

        // Handle common conditions without throwing exceptions
        private void HandleContidions()
        {
            // see Shopping.TryCheckout
        }

        // In custom exceptions, provide additional properties as needed
        private void ProvideAdditionalProperties()
        {
            // see AdultException
        }

        // Restore state when methods don't complete due to exceptions
        private void RestoreState()
        {

            // try => transaction = account.withdrawal(amount)
            // catch => account.restore(transaction)
        }
    }
}



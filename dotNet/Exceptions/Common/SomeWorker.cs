using System;

namespace Common
{
    public class SomeWorker
    {
        /// <summary>
        /// Allowed to return only completed results.
        /// </summary>
        public string ExecuteAtomic()
        {
            return ExecuteInternal();
        }

        /// <summary>
        /// Allowed to return not completed results.
        /// </summary>
        public string ExecuteDirty()
        {
            try
            {
                var result = ExecuteInternal();
                return result;
            }
            catch (Exception e)
            {
                throw new FallbackException(e.Message, "here is partial results");
            }
        }

        /// <summary>
        /// Tries to return completed results.
        /// </summary>
        public bool TryExecute(out string result)
        {
            try
            {
                result = ExecuteInternal();
                return true;
            }
            catch (Exception)
            {
                result = "partial results";
            }
            return false;
        }

        private string ExecuteInternal()
        {
            var someString = "completed result";
            throw new Exception("Can't complete operation processing.");
            //return someString;
        }

    }
}

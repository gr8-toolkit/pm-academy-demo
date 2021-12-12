using System;

namespace Common
{
    public class SomeWorker
    {
        public string Execute()
        {
            return ExecuteInternal();
        }

        public string ExecuteWithFallback()
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

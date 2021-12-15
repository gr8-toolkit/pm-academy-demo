using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class Stuff
    {
        public async Task<string> SomeExpensiveFunc(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            return "expensive-func-result";
        }

        public async Task<string> SomeUnexcpectedFunc(CancellationToken cancellationToken)
        {
            try
            {
                // task is not cancelled
                await Task.Delay(TimeSpan.FromSeconds(3));
                // cancellation is requested
                await SomeUnexcpectedConditions(cancellationToken);
            }
            catch (DllNotFoundException e)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    var tce = new TaskCanceledException();
                    throw new AggregateException(e, tce);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            return "unexcpected-func-result";
        }

        public Task SomeUnexcpectedConditions(CancellationToken cancellationToken)
        {
            //cancellationToken.ThrowIfCancellationRequested();
            throw new DllNotFoundException();
        }
    }
}

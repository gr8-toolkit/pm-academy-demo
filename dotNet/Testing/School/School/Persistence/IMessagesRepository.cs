using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace School.Persistence
{
    public interface IMessagesRepository
    {
        Task<IReadOnlyCollection<Message>> FetchUnprocessedAsync(int batchSize, CancellationToken cancellationToken);
    }
}
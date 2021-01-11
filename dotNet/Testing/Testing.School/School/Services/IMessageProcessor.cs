using System.Threading;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IMessageProcessor
    {
        Task ProcessMessagesAsync(int batchSize, CancellationToken cancellationToken);
    }
}
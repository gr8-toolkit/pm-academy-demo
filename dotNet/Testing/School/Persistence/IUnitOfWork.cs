using System.Threading;
using System.Threading.Tasks;

namespace School.Persistence
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellationToken = default);

        Task<ITransaction> BeginTransaction(CancellationToken cancellationToken = default);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace School.Persistence
{
    public interface IStudentsRepository
    {
        Task<Student> FindById(Guid id, CancellationToken cancellationToken);
        Task Create(Student student, CancellationToken cancellationToken);
    }
}
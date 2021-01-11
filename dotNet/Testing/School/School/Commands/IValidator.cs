using System.Threading;
using System.Threading.Tasks;

namespace School.Commands
{
    public interface IValidator<in TCommand>
    {
        Task<ValidationResult> ValidateAsync(TCommand command, CancellationToken cancellationToken);
    }
}
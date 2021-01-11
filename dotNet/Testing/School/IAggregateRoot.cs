using System.Collections.Generic;

namespace School
{
    public interface IAggregateRoot {
        IReadOnlyCollection<IDomainEvent> Events { get; }

        void ClearEvents();
    }
}
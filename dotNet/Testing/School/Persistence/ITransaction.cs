using System;

namespace School.Persistence
{
    public interface ITransaction : IDisposable //TODO: leaky abstraction
    {
        void Commit();

        void Rollback();
    }
}
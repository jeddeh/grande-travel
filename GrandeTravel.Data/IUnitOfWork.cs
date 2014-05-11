using System;

namespace GrandeTravel.Data
{
    public interface IUnitOfWork : IDisposable
    {
        object Context { get; }

        void StartTransaction();
        void Commit();
        void Rollback();
    }
}

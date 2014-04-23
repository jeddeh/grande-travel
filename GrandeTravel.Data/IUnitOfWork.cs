using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Data.Implementation.EntityFramework
{
    internal class UnitOfWork : IUnitOfWork
    {
        private GrandeTravelDbContext context;
        private DbContextTransaction transaction;

        public UnitOfWork(string connectionOrConfig)
        {
            context = new GrandeTravelDbContext(connectionOrConfig);
        }

        public object Context
        {
            get { return context; }
        }

        public void Commit()
        {
            context.SaveChanges();
            transaction.Commit();
        }

        public void StartTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}

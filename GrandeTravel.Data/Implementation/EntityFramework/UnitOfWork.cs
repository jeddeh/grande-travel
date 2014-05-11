using System.Data.Entity;

namespace GrandeTravel.Data.Implementation.EntityFramework
{
    internal class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        private DbContextTransaction transaction;

        public UnitOfWork(string connectionOrConfig)
        {
            context = new ApplicationDbContext(connectionOrConfig);
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

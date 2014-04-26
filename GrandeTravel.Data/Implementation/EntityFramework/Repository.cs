using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Data.Implementation.EntityFramework
{
    internal sealed class Repository<T> : IRepository<T> where T : class
    {
        private IUnitOfWork unitOfWork;
        private DbSet<T> dbSet;
        private GrandeTravelDbContext context;

        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            context = (GrandeTravelDbContext)unitOfWork.Context;
            dbSet = context.Set<T>();

            // This is a hack to ensure that the build process copies the required dll's
            // Remove this and see what happens :)
            Type dependancy = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        public T Get(object primaryKey)
        {
            return dbSet.Find(primaryKey);
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<T> Query()
        {
            return dbSet.AsQueryable();
        }

        public IQueryable<T> QueryObjectGraph(string children)
        {
            return dbSet.Include(children).AsQueryable();
        }
    }
}

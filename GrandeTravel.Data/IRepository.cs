using System.Linq;

namespace GrandeTravel.Data
{
    public interface IRepository<T> where T : class
    {
        T Get(object primaryKey);

        T Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> Query();

        IQueryable<T> QueryObjectGraph(string children);
    }
}

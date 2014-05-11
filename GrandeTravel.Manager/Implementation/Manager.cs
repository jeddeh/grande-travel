using GrandeTravel.Data;

using System;
using System.Collections.Generic;
using System.Linq;

namespace GrandeTravel.Manager.Implementation
{
    // Generic Manager implementation
    internal class Manager<T> : IManager<T> where T : class
    {
        // Fields
        private IRepository<T> repository;

        // Constructors
        public Manager(IRepository<T> repository)
        {
            this.repository = repository;
        }

        // Methods
        public T Create(T item)
        {
            return this.repository.Insert(item);
        }

        public T GetById(object id)
        {
            return repository.Get(id);
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return repository.Query().Where(predicate).ToList();
        }

        public IEnumerable<T> EagerGet(Func<T, bool> predicate, string children)
        {
            return repository.QueryObjectGraph(children).Where(predicate).ToList();
        }

        public void Update(T item)
        {
            this.repository.Update(item);
        }

        public void Delete(T item)
        {
            this.repository.Delete(item);
        }
    }
}

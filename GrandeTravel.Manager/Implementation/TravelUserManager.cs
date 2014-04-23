using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Data;
using GrandeTravel.Entity;

namespace GrandeTravel.Manager.Implementation
{
    internal class TravelUserManager : ITravelUserManager
    {
        // Fields
        private IRepository<TravelUser> repository;

        // Constructors
        public TravelUserManager(IRepository<TravelUser> repository)
        {
            this.repository = repository;
        }

        // Methods
        public void Create(TravelUser TravelUser)
        {
            this.repository.Insert(TravelUser);
        }

        public TravelUser GetById(int id)
        {
            return repository.Get(id);
        }

        public IEnumerable<TravelUser> Get(Func<Entity.TravelUser, bool> predicate)
        {
            return repository.Query().Where(predicate).ToList();
        }

        public void Update(TravelUser TravelUser)
        {
            this.repository.Update(TravelUser);
        }

        public void Delete(TravelUser TravelUser)
        {
            this.repository.Delete(TravelUser);
        }
    }
}

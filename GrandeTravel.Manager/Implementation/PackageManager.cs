using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GrandeTravel.Data;
using GrandeTravel.Entity;

namespace GrandeTravel.Manager.Implementation
{
    internal class PackageManager : IPackageManager
    {
        // Fields
        private IRepository<Package> repository;

        // Constructors
        public PackageManager(IRepository<Package> repository)
        {
            this.repository = repository;
        }

        // Methods
        public void Create(Package Package)
        {
            this.repository.Insert(Package);
        }

        public Package GetById(int id)
        {
            return repository.Get(id);
        }

        public IEnumerable<Package> Get(Func<Entity.Package, bool> predicate)
        {
            return repository.Query().Where(predicate).ToList();
        }

        public IEnumerable<Package> GetObjectGraph(Func<Entity.Package, bool> predicate, string children)
        {
            return repository.QueryObjectGraph(children).Where(predicate).ToList();
        }

        public void Update(Package package)
        {
            this.repository.Update(package);
        }

        public void Delete(Package package)
        {
            this.repository.Delete(package);
        }
    }
}

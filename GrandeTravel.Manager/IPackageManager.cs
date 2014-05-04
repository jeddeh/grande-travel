using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GrandeTravel.Entity;

namespace GrandeTravel.Manager
{
    public interface IPackageManager
    {
        Package Create(Package package);
        IEnumerable<Package> Get(Func<Package, bool> predicate);
        IEnumerable<Package> GetWithActivities(Func<Entity.Package, bool> predicate, string children);
        Package GetById(int id);
        void Update(Package package);
        void Delete(Package package);
    }
}

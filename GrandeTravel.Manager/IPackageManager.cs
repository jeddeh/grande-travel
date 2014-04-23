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
        void Create(Package package);
        IEnumerable<Package> Get(Func<Package, bool> predicate);
        Package GetById(int id);
        void Update(Package package);
        void Delete(Package package);
    }
}

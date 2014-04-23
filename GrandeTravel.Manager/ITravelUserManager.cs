using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GrandeTravel.Entity;

namespace GrandeTravel.Manager
{
    public interface ITravelUserManager
    {
        void Create(TravelUser travelUser);
        IEnumerable<TravelUser> Get(Func<TravelUser, bool> predicate);
        TravelUser GetById(int id);
        void Update(TravelUser travelUser);
        void Delete(TravelUser travelUser);
    }
}

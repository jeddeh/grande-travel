using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GrandeTravel.Entity;

namespace GrandeTravel.Manager
{
    public interface IActivityManager
    {
        void Create(Activity activity);
        IEnumerable<Activity> Get(Func<Activity, bool> predicate);
        Activity GetById(int id);
        void Update(Activity activity);
        void Delete(Activity activity);
    }

}

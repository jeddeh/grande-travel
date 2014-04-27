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
    internal class ActivityManager : IActivityManager
    {
        // Fields
        private IRepository<Activity> repository;

        // Constructors
        public ActivityManager(IRepository<Activity> repository)
        {
            this.repository = repository;
        }

        // Methods
        public void Create(Activity Activity)
        {
            this.repository.Insert(Activity);
        }

        public Activity GetById(int id)
        {
            return repository.Get(id);
        }

        public IEnumerable<Activity> Get(Func<Entity.Activity, bool> predicate)
        {
            return repository.Query().Where(predicate).ToList();
        }

        public void Update(Activity package)
        {
            this.repository.Update(package);
        }

        public void Delete(Activity package)
        {
            this.repository.Delete(package);
        }
    }
}

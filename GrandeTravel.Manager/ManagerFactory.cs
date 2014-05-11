using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Manager.Implementation;

namespace GrandeTravel.Manager
{
    public static class ManagerFactory
    {
        public static IManager<T> GetManager<T>(IRepository<T> repository) where T : class
        {
            return new Manager<T>(repository);
        }

        //public static IPackageManager GetPackageManager(IRepository<Package> repository)
        //{
        //    return new PackageManager(repository);
        //}

        //public static IApplicationUserManager GetApplicationUserManager(IRepository<ApplicationUser> repository)
        //{
        //    return new ApplicationUserManager(repository);
        //}

        //public static IActivityManager GetActivityManager(IRepository<Activity> repository)
        //{
        //    return new ActivityManager(repository);
        //}
    }
}

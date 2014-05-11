using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Manager;
using GrandeTravel.Service.Implementation;

namespace GrandeTravel.Service
{
    public static class ServiceFactory
    {
        public static IPackageService GetPackageService(IManager<Package> manager)
        {
            return new PackageService(manager);
        }

        public static IApplicationUserService GetApplicationUserService(IApplicationUserManager manager)
        {
            return new ApplicationUserService(manager);
        }

        public static IActivityService GetActivityService(IActivityManager manager)
        {
            return new ActivityService(manager);
        }
    }
}

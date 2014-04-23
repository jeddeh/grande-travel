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
        public static IPackageService GetPackageService(IPackageManager manager)
        {
            return new PackageService(manager);
        }

        public static ITravelUserService GetTravelUserService(ITravelUserManager manager)
        {
            return new TravelUserService(manager);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Manager;
using GrandeTravel.Service.Implementation;
using GrandeTravel.Entity;

namespace GrandeTravel.Service
{
    public static class ServiceFactory
    {
        public static IPackageService GetPackageService(IManager<Package> manager)
        {
            return new PackageService(manager);
        }

        public static IApplicationUserService GetApplicationUserService(IManager<ApplicationUser> manager)
        {
            return new ApplicationUserService(manager);
        }

        public static IActivityService GetActivityService(IManager<Activity> manager)
        {
            return new ActivityService(manager);
        }

        public static IOrderService GetOrderService(IManager<Order> manager)
        {
            return new OrderService(manager);
        }
    }
}

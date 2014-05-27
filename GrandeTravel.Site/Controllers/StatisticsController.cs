using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Models.Statistics;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GrandeTravel.Site.Controllers
{
    public class StatisticsController : Controller
    {
        // Fields
        private IApplicationUserService userService;
        private IPackageService packageService;
        private IOrderService orderService;

        // Constructors
        public StatisticsController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<ApplicationUser> userRepository = RepositoryFactory.GetRepository<ApplicationUser>(unitOfWork);
            IManager<ApplicationUser> applicationUserManager = ManagerFactory.GetManager(userRepository);
            this.userService = ServiceFactory.GetApplicationUserService(applicationUserManager);

            IRepository<Package> packageRepository = RepositoryFactory.GetRepository<Package>(unitOfWork);
            IManager<Package> packageManager = ManagerFactory.GetManager(packageRepository);
            this.packageService = ServiceFactory.GetPackageService(packageManager);

            IRepository<Order> orderRepository = RepositoryFactory.GetRepository<Order>(unitOfWork);
            IManager<Order> orderManager = ManagerFactory.GetManager(orderRepository);
            this.orderService = ServiceFactory.GetOrderService(orderManager);
        }

        // Methods
        [Authorize(Roles = "ActiveUser")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            StatisticsViewModel model = new StatisticsViewModel();
            model.RegisteredUsersByState = new Dictionary<AustralianStateEnum, int>();
            model.AveragePackagePriceByState = new Dictionary<AustralianStateEnum, decimal>();

            try
            {
                // Get number of registered users by state
                Result<IEnumerable<ApplicationUser>> userResult = new Result<IEnumerable<ApplicationUser>>();
                userResult = userService.GetAllUsers();

                if (userResult.Status == ResultEnum.Success)
                {
                    var usersByState = from user in userResult.Data
                                       group user by user.State
                                           into newUser
                                           select new
                                           {
                                               State = newUser.Key,
                                               Count = newUser.Count()
                                           };

                    model.RegisteredUsersByState = usersByState.ToDictionary(p => p.State, p => p.Count);
                }
            }
            catch
            {
                // Query failed
            }

            try
            {
                // Get average package price by state
                Result<IEnumerable<Package>> packageResult = new Result<IEnumerable<Package>>();
                packageResult = packageService.GetAllPackages(false);

                if (packageResult.Status == ResultEnum.Success)
                {
                    var averagePackagePriceByState = from package in packageResult.Data
                                                     group package by package.State
                                                         into newPackage
                                                         select new
                                                         {
                                                             State = newPackage.Key,
                                                             AveragePrice = newPackage.Average(p => p.Amount)
                                                         };

                    model.AveragePackagePriceByState = averagePackagePriceByState.ToDictionary(p => p.State, p => p.AveragePrice);
                }
            }
            catch
            {
                // Query failed
            }

            try
            {
                // Get revenue by year
                Result<IEnumerable<Order>> orderResult = new Result<IEnumerable<Order>>();
                orderResult = orderService.GetAllOrders();

                if (orderResult.Status == ResultEnum.Success)
                {
                    var revenueByYear = from order in orderResult.Data
                                        group order by order.DateBooked.Year
                                            into newOrder
                                            orderby newOrder.Key
                                            select new
                                            {
                                                Year = newOrder.Key,
                                                Revenue = newOrder.Sum(p => p.Amount)
                                            };


                    model.RevenueByYear = revenueByYear.ToDictionary(p => p.Year, p => p.Revenue);
                }
            }
            catch
            {
                return View(model);
            }

            return View(model);
        }
    }
}
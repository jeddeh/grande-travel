using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles = "ActiveUser")]
    public class OrdersController : Controller
    {

        // Fields
        private IOrderService orderService;

        // Constructors
        public OrdersController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Order> repository = RepositoryFactory.GetRepository<Order>(unitOfWork);
            IManager<Order> orderManager = ManagerFactory.GetManager(repository);
            this.orderService = ServiceFactory.GetOrderService(orderManager);
        }

        #region Search Orders

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Search()
        {
            CustomerOrdersViewModel model = new CustomerOrdersViewModel();

            int customerId = WebSecurity.CurrentUserId;
            Result<IEnumerable<Order>> result = orderService.GetOrdersByCustomerId(customerId);

            switch (result.Status)
            {
                case ResultEnum.Success:
                    model.Orders = new List<Order>();
                    break;

                case ResultEnum.Fail:
                    break;

                default:
                    break;
            }

            return View(model);
        }

        #endregion
    }
}
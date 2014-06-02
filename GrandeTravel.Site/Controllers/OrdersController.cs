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
            model.Orders = new List<Order>();

            try
            {
                int customerId = WebSecurity.CurrentUserId;
                Result<IEnumerable<Order>> result = orderService.GetOrdersByCustomerId(customerId);

                switch (result.Status)
                {
                    case ResultEnum.Success:
                        model.Orders = result.Data.ToList<Order>();
                        break;

                    case ResultEnum.Fail:
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                return View(model);
            }

            return View(model);
        }

        #endregion

        #region Feedback

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Feedback(int orderId)
        {
            FeedbackViewModel model = new FeedbackViewModel();
            string errorMessage = "Sorry, we are currently unable to save feedback for this order.";

            try
            {
                Result<Order> result = orderService.GetOrderById(orderId);

                switch (result.Status)
                {
                    case ResultEnum.Success:
                        model.OrderId = orderId;
                        model.PackageName = result.Data.Package.Name;
                        model.Feedback = result.Data.Feedback;
                        break;

                    case ResultEnum.Fail:
                        ModelState.AddModelError("ErrorMessage", errorMessage);
                        break;

                    default:
                        ModelState.AddModelError("ErrorMessage", errorMessage);
                        break;
                }
            }
            catch
            {
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(FeedbackViewModel model)
        {
            Order order = new Order();
            string errorMessage = "Sorry, we are currently unable to save feedback for this order.";

            if (ModelState.IsValid)
            {
                // Get the Order
                try
                {
                    Result<Order> result = orderService.GetOrderById(model.OrderId);

                    if (result.Status != ResultEnum.Success)
                    {
                        ModelState.AddModelError("ErrorMessage", errorMessage);
                        return View(model);
                    }
                }
                catch
                {
                    ModelState.AddModelError("ErrorMessage", errorMessage);
                    return View(model);
                }

                // Update Order
                try
                {
                    order.Feedback = model.Feedback;
                    ResultEnum result = orderService.UpdateOrder(order);

                    if (result != ResultEnum.Success)
                    {
                        ModelState.AddModelError("ErrorMessage", errorMessage);
                        return View(model);
                    }

                    ViewData["SuccessMessage"] = "Thankyou for your feedback.";
                    return View(model);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("ErrorMessage", errorMessage);
                    return View(model);
                }
            }
        }
        #endregion
    }
}
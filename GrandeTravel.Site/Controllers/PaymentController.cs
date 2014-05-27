using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Helpers;
using GrandeTravel.Site.Helpers.Mappers;
using GrandeTravel.Site.Models.Payment;
using GrandeTravel.Utility;
using GrandeTravel.Utility.Helpers;
using System;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles = "Customer")]
    [Authorize(Roles = "ActiveUser")]
    public class PaymentController : Controller
    {
        // Fields
        private IPackageService packageService;
        private IOrderService orderService;
        private IApplicationUserService userService;

        // Constructors
        public PaymentController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Package> packageRepository = RepositoryFactory.GetRepository<Package>(unitOfWork);
            IManager<Package> packageManager = ManagerFactory.GetManager(packageRepository);
            this.packageService = ServiceFactory.GetPackageService(packageManager);

            IRepository<Order> orderRepository = RepositoryFactory.GetRepository<Order>(unitOfWork);
            IManager<Order> orderManager = ManagerFactory.GetManager(orderRepository);
            this.orderService = ServiceFactory.GetOrderService(orderManager);

            IRepository<ApplicationUser> applicationUserRepository = RepositoryFactory.GetRepository<ApplicationUser>(unitOfWork);
            IManager<ApplicationUser> applicationUserManager = ManagerFactory.GetManager(applicationUserRepository);
            this.userService = ServiceFactory.GetApplicationUserService(applicationUserManager);
        }

        // Methods
        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult CreateTransaction(int? PackageId)
        {
            // Get the package
            PaymentViewModel model = new PaymentViewModel();

            try
            {
                if (PackageId == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                int packageId = PackageId.GetValueOrDefault();

                Result<Package> packageResult = new Result<Package>();
                packageResult = packageService.GetPackageById(packageId);

                if (packageResult.Status == ResultEnum.Success && packageResult.Data.Status == PackageStatusEnum.Available)
                {
                    if (MvcApplication.ShowSampleFormData)
                    {
                        // Show dummy user data for model
                        model = SampleModelData.GetSamplePaymentViewModel();
                    }

                    model.PackageId = packageId;
                    model.PackageName = packageResult.Data.Name;
                    model.Amount = packageResult.Data.Amount;

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateTransaction(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Order order;
            ApplicationUser user;
            Payment payment;
            PaymentResult paymentResult;
            string orderError = "Sorry, we were unable to process your order. Your credit card has not been charged.";
           
            try
            {
                // Get ApplicationUser
                Result<ApplicationUser> userResult = userService.GetApplicationUserById(WebSecurity.CurrentUserId);

                if (userResult.Status != ResultEnum.Success)
                {
                    ModelState.AddModelError("ErrorMessage", orderError);
                    return View(model);
                }

                user = userResult.Data;
                       
                // Create Order
                Result<Order> orderResult = orderService.AddOrder(new Order
                {
                    PackageId = model.PackageId,
                    Amount = model.Amount,
                    CustomerId = WebSecurity.CurrentUserId,
                    DateBooked = DateTime.Now,
                    Paid = false
                });

                if (orderResult.Status != ResultEnum.Success)
                {
                    ModelState.AddModelError("ErrorMessage", orderError);
                    return View(model);
                }

                order = orderResult.Data;
            }
            catch
            {
                ModelState.AddModelError("ErrorMessage", orderError);
                return View(model);
            }

            // Submit Payment
            string paymentError = "Unable to process payment. Please contact us.";

            try
            {
                payment = model.ToPayment();
                IPaymentService paymentService = UtilityFactory.GetBrainTreeService(Authentication.GetBrainTreeAuthentication());
                paymentResult = paymentService.SubmitPayment(payment);
                if (!paymentResult.IsSuccess)
                {
                    ModelState.AddModelError("ErrorMessage", paymentError);
                    return View(model);
                }
            }
            catch
            {
                ModelState.AddModelError("ErrorMessage", paymentError);
                return View(model);
            }

            // Payment Successful
            try
            {
                // Update Order
                order.Paid = true;
                order.TransactionId = paymentResult.TransactionId;
                order.VoucherCode = Guid.NewGuid();

                ResultEnum result = orderService.UpdateOrder(order);

                if (result != ResultEnum.Success)
                {
                    // Payment succeeded, but database update failed.
                    // TODO : Log error
                }

                // Send SMS
                string phoneNumber = PhoneValidation.ValidateMobileNumber(user.Phone);

                if (phoneNumber != null)
                {
                    string message = String.Format(
                        "Hi {0}, Congratulations on your successful order of our {1} package. Enjoy your trip!",
                        user.FirstName,
                        model.PackageName);

                    GrandeTravel.Utility.IPhoneService commClient =
                        UtilityFactory.GetPhoneService(Authentication.GetTwilioAuthentication());

                    commClient.SendSMS(phoneNumber, message);
                }

                // Send Email
                IEmailService emailService = UtilityFactory.GetEmailService(Authentication.GetDefaultEmailAuthentication());

                string crlf = "<br />";
                DateTime expiryDate = DateTime.Today.AddMonths(3);

                Email email = new Email
                {
                    // Unique voucher code, package details, and expiry date which are 3 months from the date of payment.

                    From = Authentication.GetDefaultEmailSenderAddress(),
                    To = WebSecurity.CurrentUserName,
                    Subject = "Grande Travel Package Details",
                    
                    Body = String.Format(
                        "Hi {1}, {0}{0}" +
                        "Your payment of {2} for our {3} package has been successful. {0}" + 
                        "Your credit card transaction code is {4}. {0}{0}"+ 
                        "Your Grande Travel voucher code is {5}, which is redeemable until {6}.{0}",
                        crlf,
                        user.FirstName,
                        String.Format("{0:c}" ,model.Amount),
                        model.PackageName, 
                        order.TransactionId, 
                        order.VoucherCode,
                        expiryDate.ToLongDateString())
                };

                emailService.SendEmail(email);

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }
    }
}

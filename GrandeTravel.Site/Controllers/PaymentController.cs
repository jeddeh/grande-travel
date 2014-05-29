using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Helpers;
using GrandeTravel.Site.Models.Payment;
using GrandeTravel.Utility;
using GrandeTravel.Utility.Helpers;
using System;
using System.IO;
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

        private ApplicationUser user = new ApplicationUser();
        private Package package = new Package();
        private PaymentResult paymentResult = new PaymentResult();

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
        #region Create Transaction

        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult CreateTransaction(int? PackageId)
        {
            // Get the package
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
                    // No Model is used in order to prevent a name attribute being applied to form inputs -
                    // this causes unencrypted form fields to be posted.
                    TempData.Clear();
                    TempData.Add("Package", packageResult.Data);
                    TempData.Keep("Package");
                    return View();
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
        public ActionResult CreateTransaction(FormCollection collection)
        {
            string resultMessage = "The transaction could not be processed.";

            // FormCollection is needed in order to use client-side encryption.
            if (collection.Count <= 1)
            {
                // Encryption failed / form not valid - go to failure page 
                LogTransaction("Form fields not received.", false);
                return RedirectToResult(resultMessage, false);
            }

            var packageHolder = TempData["Package"];

            if (packageHolder == null)
            {
                // Go to failure page - card not charged
                LogTransaction("No package information received.", false);
                return RedirectToResult(resultMessage, false);
            }

            Order order;
            Payment payment;
            package = (Package)packageHolder;

            try
            {
                // Get ApplicationUser
                Result<ApplicationUser> userResult = userService.GetApplicationUserById(WebSecurity.CurrentUserId);

                if (userResult.Status != ResultEnum.Success)
                {
                    // Go to failure page - card not charged
                    LogTransaction("Unable to get user details.", false);
                    return RedirectToResult(resultMessage, false);
                }

                user = userResult.Data;

                // Create Order
                Result<Order> orderResult = orderService.AddOrder(new Order
                {
                    PackageId = package.PackageId,
                    Amount = package.Amount,
                    CustomerId = WebSecurity.CurrentUserId,
                    DateBooked = DateTime.Now,
                    Paid = false
                });

                if (orderResult.Status != ResultEnum.Success)
                {
                    // Go to failure page - card not charged
                    LogTransaction("Unable to get order details.", false);
                    return RedirectToResult(resultMessage, false);
                }

                order = orderResult.Data;
            }
            catch (Exception e)
            {
                // Go to failure page - card not charged
                LogTransaction(e.Message, false);
                return RedirectToResult(resultMessage, false);
            }

            // Submit Payment
            try
            {
                payment = new Payment
                {
                    CCNumber = collection["number"],
                    CVV = collection["cvv"],
                    ExpirationMonth = collection["month"],
                    ExpirationYear = collection["year"],
                    Amount = package.Amount,
                    PackageId = package.PackageId,
                    PackageName = package.Name
                };

                IPaymentService paymentService = UtilityFactory.GetBrainTreeService(Authentication.GetBrainTreeAuthentication());
                paymentResult = paymentService.SubmitPayment(payment);
                if (!paymentResult.IsSuccess)
                {
                    LogTransaction("Error submitting payment.", false);
                    return RedirectToResult(resultMessage, false);
                }
            }
            catch (Exception e)
            {
                LogTransaction(e.Message, false);
                return RedirectToResult(resultMessage, false);
            }

            // Payment Successful
            resultMessage = "Your transaction has been processed. Enjoy your holiday!";

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
                    LogTransaction("Failed to update database.", true);
                }
            }
            catch (Exception e)
            {
                // Payment succeeded, but database update failed.
                LogTransaction(e.Message, true);
                resultMessage = "Your transaction has been processed. Please contact us about your trip.";
            }

            try
            {
                // Send SMS
                string phoneNumber = PhoneValidation.ValidateMobileNumber(user.Phone);

                if (phoneNumber != null)
                {
                    string smsMessage = String.Format(
                        "Hi {0}, Congratulations on your successful order of our {1} package. Enjoy your trip!",
                        user.FirstName,
                        package.Name);

                    GrandeTravel.Utility.IPhoneService commClient =
                        UtilityFactory.GetPhoneService(Authentication.GetTwilioAuthentication());

                    commClient.SendSMSAsync(phoneNumber, smsMessage);
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
                        "Your credit card transaction code is {4}. {0}{0}" +
                        "Your Grande Travel voucher code is {5}, which is redeemable until {6}.{0}",
                        crlf,
                        user.FirstName,
                        String.Format("{0:c}", package.Amount),
                        package.Name,
                        order.TransactionId,
                        order.VoucherCode,
                        expiryDate.ToLongDateString())
                };

                emailService.SendEmailAsync(email);
            }
            catch (Exception e)
            {
                // Email or Sms failed - but this will not catch async errors.
                LogTransaction(e.Message, true);
                resultMessage = "Your transaction has been processed. <br /> Please contact us about your trip.";
                return RedirectToResult(resultMessage, true);
            }

            LogTransaction("Successful Purchase.", true);
            return RedirectToResult(resultMessage, true);
        }

        public RedirectToRouteResult RedirectToResult(string message, bool isSuccess)
        {
            PaymentResultViewModel model = new PaymentResultViewModel
            {
                IsSuccess = isSuccess,
                Message = message
            };

            TempData.Clear();
            TempData["PaymentResultViewModel"] = model;
            return RedirectToAction("Result");
        }

        #endregion

        #region Payment Result

        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        [HttpGet]
        public ActionResult Result()
        {
            PaymentResultViewModel viewModel = (PaymentResultViewModel)TempData["PaymentResultViewModel"];

            if (viewModel == null)
            {
                RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        #endregion

        #region Log Transaction

        public void LogTransaction(string message, bool isSuccess)
        {
            try
            {
                using (StreamWriter sw =new StreamWriter(Server.MapPath("~/Log/TransactionLog.txt"), true))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(message);
                    sw.WriteLine();
                }
            }
            catch (Exception e)
            {
                // Log failed
            }
        }
        #endregion
    }
}

using GrandeTravel.Entity;
using GrandeTravel.Site.Models.Payment;
using GrandeTravel.Site.Helpers.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrandeTravel.Utility;
using GrandeTravel.Utility.Implementation;
using System.Configuration;
using System.Web.Configuration;
using GrandeTravel.Service;
using GrandeTravel.Data;
using GrandeTravel.Manager;
using GrandeTravel.Site.Helpers;

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles = "Customer")]
    [Authorize(Roles = "ActiveUser")]
    public class PaymentController : Controller
    {
        // Fields
        private IPackageService packageService;

        // Constructors
        public PaymentController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Package> repository = RepositoryFactory.GetRepository<Package>(unitOfWork);

            IManager<Package> packageManager = ManagerFactory.GetManager(repository);

            this.packageService = ServiceFactory.GetPackageService(packageManager);
        }

        // Methods
        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult CreateTransaction(int? packageId)
        {
            if (packageId == null)
            {
                RedirectToAction("Index", "Home");
            }

            PaymentViewModel model = new PaymentViewModel();

            if (MvcApplication.ShowSampleFormData)
            {
                // Show dummy user data for model
                model = SampleModelData.GetSamplePaymentViewModel();
            }

            return View(model);
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

            try
            {
                // Payment
                Payment payment = model.ToPayment();
                IPaymentService paymentService = UtilityFactory.GetBrainTreeService(AuthenticationFactory.GetBrainTreeAuthentication());
                PaymentResult result = paymentService.SubmitPayment(payment);
            }
            catch
            {
                ModelState.AddModelError("ErrorMessage", "Unable to process payment. Please contact us.");
                return View(model);
            }

            try
            {
                // Send Email
                IEmailService emailService = UtilityFactory.GetEmailService(AuthenticationFactory.GetDefaultEmailAuthentication());

                Email email = new Email
                {
                    From = "grandetraveller@gmail.com",
                    To = "robgrantj@yahoo.com.au",
                    Subject = "Test",
                    Body = "Body"
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

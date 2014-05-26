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

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles="Customer")]
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

            PaymentViewModel model = new PaymentViewModel
            {
                PackageId = 1,
                PackageName = "The first package",
                Amount = 500m
            };

            return View(model);
        }

        [Authorize(Roles="Customer")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateTransaction(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Payment payment = model.ToPayment();

            IPaymentService paymentService = UtilityFactory.GetBrainTreeService(new BrainTreeAuthentication
                {
                    MerchantId = WebConfigurationManager.AppSettings["brainTreeMerchantId"],
                    PrivateKey = WebConfigurationManager.AppSettings["brainTreePrivateKey"],
                    PublicKey = WebConfigurationManager.AppSettings["brainTreePublicKey"]
                });

            PaymentResult result = paymentService.SubmitPayment(payment);

            return View();
        }
    }
}

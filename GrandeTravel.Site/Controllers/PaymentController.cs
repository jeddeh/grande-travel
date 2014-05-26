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

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles="Customer")]
    [Authorize(Roles = "ActiveUser")]
    public class PaymentController : Controller
    {
        [Authorize(Roles = "Customer")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Index()
        {
            return View();
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
                    MerchantId = "",
                    PrivateKey = "",
                    PublicKey = ""
                });

            PaymentResult result = paymentService.SubmitPayment(payment);

            return View();
        }
    }
}

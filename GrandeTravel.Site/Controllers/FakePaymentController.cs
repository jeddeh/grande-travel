using GrandeTravel.Entity;
using GrandeTravel.Site.Helpers;
using GrandeTravel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrandeTravel.Site.Controllers
{
    public class FakePaymentController : Controller
    {
        public ActionResult CreateTransaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTransaction(FormCollection collection)
        {
            Payment payment = new Payment
            {
                Amount = 1000.00m,
                CCNumber = collection["number"],
                CVV = collection["cvv"],
                ExpirationMonth = collection["month"],
                ExpirationYear = collection["year"]
            };

            IPaymentService paymentService = UtilityFactory.GetBrainTreeService(Authentication.GetBrainTreeAuthentication());
            PaymentResult paymentResult = paymentService.SubmitPayment(payment);
   
            if (paymentResult.IsSuccess)
            {
               
            }
            else
            {
               
            }

            return View();
        }
    }
}

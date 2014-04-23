using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    public class LogoutController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                WebSecurity.Logout();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

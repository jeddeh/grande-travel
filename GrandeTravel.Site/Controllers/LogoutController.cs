using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    [Authorize]
    public class LogoutController : Controller
    {
        #region Logout

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

        #endregion
    }
}

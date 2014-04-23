using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GrandeTravel.Entity;

namespace GrandeTravel.Site.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}

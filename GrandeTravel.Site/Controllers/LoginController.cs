using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GrandeTravel.Entity;
using GrandeTravel.Site.Models;
using GrandeTravel.Entity.Enums;
using WebMatrix.WebData;
using System.Web.Security;

namespace GrandeTravel.Site.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userLogin = model.Email.ToLower();

                    if (WebSecurity.UserExists(userLogin) && Roles.GetRolesForUser(userLogin).Length == 0)
                    {
                        // User is inactive
                        ModelState.AddModelError("ErrorMessage", "Your account has been disabled. Please contact us for further information.");
                        return View(model);
                    }

                    if (WebSecurity.Login(userLogin, model.Password))
                    {
                        // Login successful
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Login unsuccessful
                        ModelState.AddModelError("ErrorMessage", "The email or password is incorrect");
                        return View(model);
                    }
                }
                catch
                {
                    ModelState.AddModelError("ErrorMessage", "Unable to log in. Please contact us for assistance.");
                    return View();
                }
            }
            ModelState.AddModelError("ErrorMessage", "Unable to log in. Please contact us for assistance.");
            return View(model);
        }
    }
}

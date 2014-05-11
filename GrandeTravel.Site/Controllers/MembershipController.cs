using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Mappers;
using GrandeTravel.Site.Models;
using WebMatrix.Data;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        // Fields
        private IApplicationUserService userService;

        // Constructors
        public MembershipController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<ApplicationUser> repository = RepositoryFactory.GetRepository<ApplicationUser>(unitOfWork);

            IApplicationUserManager ApplicationUserManager = ManagerFactory.GetApplicationUserManager(repository);

            this.userService = ServiceFactory.GetApplicationUserService(ApplicationUserManager);
        }

        // Methods
        #region Add User

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Add()
        {
            // Dummy user data for model
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);

            MembershipViewModel model = new RegisterUserViewModel
            {
                Address = "5 Short Street",
                City = "Sydney",
                ConfirmPassword = "111111",
                Email = "andrewjones" + randomNumber + "@aj.com.au",
                FirstName = "Andrew",
                LastName = "Jones",
                Password = "111111",
                Phone = "123456789",
                Postcode = "2016",
                State = AustralianStateEnum.WA
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Add(RegisterUserViewModel model)
        {
            string errorMessage = "Unable to register.  Please contact us for assistance.";

            if (ModelState.IsValid)
            {
                try
                {
                    string userLogin = model.Email.ToLower();

                    if (WebSecurity.UserExists(userLogin))
                    {
                        ModelState.AddModelError("EmailAlreadyExists", "The Email address is already in use.");
                        return View(model);
                    }

                    WebSecurity.CreateUserAndAccount(userLogin, model.Password);

                    ApplicationUser user = model.ToApplicationUser();
                    user.ApplicationUserId = WebSecurity.GetUserId(userLogin);
                    user.Email = userLogin;

                    ResultEnum result = userService.CreateApplicationUser(user);
                    switch (result)
                    {
                        case ResultEnum.Success:
                            if (model.IsProvider)
                            {
                                Roles.AddUserToRole(userLogin, "Provider");
                            }
                            else
                            {
                                Roles.AddUserToRole(userLogin, "Customer");
                            }

                            if (WebSecurity.Login(model.Email, model.Password))
                            {
                                // Login successful
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                // Login unsuccessful
                                ModelState.AddModelError("ErrorMessage", errorMessage);
                                return View(model);
                            }

                        case ResultEnum.Fail:
                            break;
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ErrorMessage", errorMessage);
                    return View(model);
                }
            }

            return View(model);
        }

        #endregion

        #region Edit User

        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            string errorMessage = "We were unable to retrieve your account details.";
            MembershipViewModel model = new EditUserViewModel();

            // Get user data for model
            try
            {
                int userId = WebSecurity.CurrentUserId;

                Result<ApplicationUser> result = userService.GetApplicationUserById(userId);

                switch (result.Status)
                {
                    case ResultEnum.Success:
                        model = result.Data.ToMembershipViewModel<EditUserViewModel>();
                        return View(model);

                    case ResultEnum.Fail:
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return View(model);
            }

            ModelState.AddModelError("ErrorMessage", errorMessage);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(EditUserViewModel model)
        {
            string errorMessage = "Sorry, we were unable to edit your account.";

            if (ModelState.IsValid)
            {
                try
                {
                    int userId = WebSecurity.CurrentUserId;
                    string userLogin = WebSecurity.CurrentUserName;

                    if (model.Password != null)
                    {
                        // Change password
                        try
                        {
                            string tempToken = WebSecurity.GeneratePasswordResetToken(userLogin);
                            WebSecurity.ResetPassword(tempToken, model.Password);
                            ViewBag.Message = "Your password has been changed. ";
                        }
                        catch
                        {
                            ViewBag.Message = "We were unable to change your password. ";
                        }
                    }

                    ApplicationUser user = model.ToApplicationUser();
                    user.ApplicationUserId = WebSecurity.GetUserId(userLogin);
                    user.Email = userLogin;

                    ResultEnum result = userService.UpdateApplicationUser(user);
                    switch (result)
                    {
                        case ResultEnum.Success:
                            ViewBag.Message += "Your account details have been updated.";
                            return View(model);

                        case ResultEnum.Fail:
                            ModelState.AddModelError("ErrorMessage", errorMessage);
                            return View(model);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("ErrorMessage", errorMessage);
                    return View(model);
                }
            }

            return View(model);
        }

        #endregion

        #region Validate Email

        [AllowAnonymous]
        public string ValidateEmail(string email)
        {
            try
            {
                string userLogin = email.ToLower();

                if (WebSecurity.UserExists(userLogin))
                {
                    return "Invalid";
                }
            }
            catch
            {
                return "Error";
            }

            return "Valid";
        }

        #endregion
    }
}

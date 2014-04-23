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
    public class MembershipController : Controller
    {
        private ITravelUserService userService;

        // Constructors
        public MembershipController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DBConnectionString");

            IRepository<TravelUser> repository = RepositoryFactory.GetRepository<TravelUser>(unitOfWork);

            ITravelUserManager travelUserManager = ManagerFactory.GetTravelUserManager(repository);

            this.userService = ServiceFactory.GetTravelUserService(travelUserManager);
        }

        // Methods
        #region Create User

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
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

            ViewBag.MembershipMode = "Register";
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(RegisterUserViewModel model)
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
                    TravelUser user = new TravelUser
                    {
                        TravelUserId = WebSecurity.GetUserId(userLogin),
                        Email = userLogin,
                        Address = model.Address,
                        City = model.City,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Postcode = model.Postcode,
                        State = model.State
                    };

                    ResultEnum result = userService.CreateTravelUser(user);
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

                Result<TravelUser> result = userService.GetTravelUserById(userId);

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

                    TravelUser user = new TravelUser
                    {
                        TravelUserId = userId,
                        Email = userLogin,
                        Address = model.Address,
                        City = model.City,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Postcode = model.Postcode,
                        State = model.State
                    };

                    ResultEnum result = userService.UpdateTravelUser(user);
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
    }
}

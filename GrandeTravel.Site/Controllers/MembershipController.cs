using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Helpers.Mappers;
using GrandeTravel.Site.Models.Membership;

using PagedList;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles = "ActiveUser")]
    public class MembershipController : Controller
    {
        // Fields
        private IApplicationUserService userService;

        // Constructors
        public MembershipController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<ApplicationUser> repository = RepositoryFactory.GetRepository<ApplicationUser>(unitOfWork);

            IManager<ApplicationUser> applicationUserManager = ManagerFactory.GetManager(repository);

            this.userService = ServiceFactory.GetApplicationUserService(applicationUserManager);
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

            // Set IsAdmin property on model
            if (Roles.IsUserInRole("Admin"))
            {
                model.IsAdmin = true;
            }
            else
            {
                model.IsAdmin = false;
            }

            return View(model);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(RegisterUserViewModel model)
        {
            string errorMessage = "Unable to register. Please contact us for assistance.";

            if (ModelState.IsValid)
            {
                try
                {
                    if (Roles.IsUserInRole("Admin"))
                    {
                        model.IsAdmin = true;
                    }

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
                                Roles.AddUserToRoles(userLogin, new string[] { "Provider", "ActiveUser" });
                            }
                            else
                            {
                                Roles.AddUserToRoles(userLogin, new string[] { "Customer", "ActiveUser" });
                            }

                            if (!Roles.IsUserInRole("Admin"))
                            {
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
                            }
                            else
                            {
                                // Admin user - Create user only. Show success message, but do not log in.
                                model.AccountCreatedSuccessfully = true;
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
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Edit(int? user)
        {
            EditUserViewModel model = new EditUserViewModel();
            int userId;
            bool userStatus;
            bool isAdminEdit;

            string errorMessage = (user != null) ? "Unable to retrieve the account details." :
                    "We were unable to retrieve your account details.";

            // Get user data for model
            try
            {
                if (user != null)
                {
                    isAdminEdit = true;
                    userId = user.GetValueOrDefault();
                }
                else
                {
                    isAdminEdit = false;
                    userId = WebSecurity.CurrentUserId;
                }

                Result<ApplicationUser> result = userService.GetApplicationUserById(userId);

                switch (result.Status)
                {
                    case ResultEnum.Success:
                        model = result.Data.ToMembershipViewModel<EditUserViewModel>();
                        model.UserId = userId;
                        model.IsAdminEdit = isAdminEdit;
                        model.IsInactiveUser = !Roles.GetRolesForUser(model.Email).Contains("ActiveUser");
                        
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

        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(EditUserViewModel model)
        {
            string errorMessage = "Sorry, we were unable to edit your account.";

            if (ModelState.IsValid)
            {
                try
                {
                    int userId;
                    string userLogin;

                    if (model.IsAdminEdit)
                    {
                        userId = model.UserId;
                        userLogin = model.Email;

                        if (model.IsInactiveUser && Roles.IsUserInRole("ActiveUser"))
                        {
                            Roles.RemoveUserFromRole(model.Email, "ActiveUser");
                        }
                        else if (!Roles.IsUserInRole("ActiveUser"))
                        {
                            Roles.AddUserToRole(model.Email, "ActiveUser");
                        }
                    }
                    else
                    {
                        userId = WebSecurity.CurrentUserId;
                        userLogin = WebSecurity.CurrentUserName;
                    }

                    if (model.Password != null)
                    {
                        // Change password
                        try
                        {
                            string tempToken = WebSecurity.GeneratePasswordResetToken(userLogin);
                            WebSecurity.ResetPassword(tempToken, model.Password);
                            ViewBag.Message = model.IsAdminEdit ? "The password has been changed. " :
                                "Your password has been changed. ";
                        }
                        catch
                        {
                            ViewBag.Message = model.IsAdminEdit ? "Unable to change the password. " :
                                "We were unable to change your password. ";
                        }
                    }

                    ApplicationUser user = model.ToApplicationUser();
                    user.ApplicationUserId = WebSecurity.GetUserId(userLogin);
                    user.Email = userLogin;

                    ResultEnum result = userService.UpdateApplicationUser(user);
                    switch (result)
                    {
                        case ResultEnum.Success:
                            ViewBag.Message += model.IsAdminEdit ? "The account details have been updated." :
                                "Your account details have been updated.";

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

        #region Search Users

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Search(int? page)
        {
            int pageNumber = page ?? 1;
            SearchUserViewModel model = new SearchUserViewModel();

            try
            {
                // TODO : Better to move paging to the Manager project.
                Result<IEnumerable<ApplicationUser>> result = userService.GetAllUsers();

                switch (result.Status)
                {
                    case ResultEnum.Success:
                        IEnumerable<ApplicationUser> pagedList = result.Data.ToPagedList<ApplicationUser>(pageNumber, 2);
                        IEnumerable<PagedUserViewModel> pagedUsers = pagedList.ToPagedUserViewModels();
                        model.PagedList = pagedList;
                        model.PagedUsers = pagedUsers;

                        // Get the roles for each user
                        foreach (PagedUserViewModel userModel in pagedUsers)
                        {
                            string[] userRoles = Roles.GetRolesForUser(userModel.Email);
                            if (userRoles.Contains("ActiveUser"))
                            {
                                userModel.IsActive = true;
                            }
                            else
                            {
                                userModel.IsActive = false;
                            }

                            if (userRoles.Contains("Admin"))
                            {
                                userModel.Role = "Admin";
                            }
                            else if (userRoles.Contains("Provider"))
                            {
                                userModel.Role = "Provider";
                            }
                            else if (userRoles.Contains("Customer"))
                            {
                                userModel.Role = "Customer";
                            }
                        }

                        return View(model);

                    case ResultEnum.Fail:
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                ModelState.AddModelError("ErrorMessage", "Fail");
                return View(model);
            }

            ModelState.AddModelError("ErrorMessage", "Fail");
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

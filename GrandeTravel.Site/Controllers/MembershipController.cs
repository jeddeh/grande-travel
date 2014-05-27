using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Utility;
using GrandeTravel.Utility.Implementation;
using GrandeTravel.Site.Helpers.Mappers;
using GrandeTravel.Site.Models.Membership;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using GrandeTravel.Utility.Helpers;
using System.Web.Configuration;
using GrandeTravel.Site.Helpers;

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
            MembershipViewModel model = new RegisterUserViewModel();

            if (MvcApplication.ShowSampleFormData)
            {
                // Show dummy user data for model
                model = SampleModelData.GetSampleRegisterViewModel();
            }

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string errorMessage = "Unable to register. Please contact us for assistance.";

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

                                // Send SMS message to confirm successful registration
                                string phoneNumber = PhoneValidation.ValidateMobileNumber(model.Phone);

                                if (phoneNumber != null)
                                {
                                    string message = String.Format(
    "Hi {0}, We're just confirming your successful registration with Grande Travel.", model.FirstName);

                                    GrandeTravel.Utility.IPhoneService commClient =
                                        UtilityFactory.GetPhoneService(Authentication.GetTwilioAuthentication());

                                    commClient.SendSMS(phoneNumber, message);
                                }

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

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string errorMessage = "Sorry, we were unable to edit your account.";

            try
            {
                int userId;
                string userLogin;

                if (model.IsAdminEdit)
                {
                    userId = model.UserId;
                    userLogin = model.Email;

                    if (model.IsInactiveUser && Roles.IsUserInRole(userLogin, "ActiveUser"))
                    {
                        Roles.RemoveUserFromRole(userLogin, "ActiveUser");
                    }
                    else if (!Roles.IsUserInRole(userLogin, "ActiveUser"))
                    {
                        Roles.AddUserToRole(userLogin, "ActiveUser");
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

            return View(model);
        }

        #endregion

        #region Search Users

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Search(string filter, string searchText, int? Page)
        {
            int page = Page ?? 1;
            SearchUserViewModel model = new SearchUserViewModel();

            try
            {
                // TODO : Better to move paging to the Manager project. 
                // Otherwise, cache the IEnumerable<ApplicationUser> so that
                // the entire collection does not have to be re-queried when changing page.
                Result<IEnumerable<ApplicationUser>> result = new Result<IEnumerable<ApplicationUser>>();

                if (String.IsNullOrEmpty(searchText) || !Enum.GetNames(typeof(SearchUserEnum)).Contains(filter))
                {
                    // Show all accounts
                    model.SearchHeading = "Showing all accounts";
                    result = userService.GetAllUsers();
                }
                else
                {
                    // Filter accounts
                    model.SearchCriteria = (SearchUserEnum)Enum.Parse(typeof(SearchUserEnum), filter);
                    model.SearchText = searchText;
                    model.SearchHeading = "Showing accounts with " + filter + " containing \"" + searchText + "\"";
                    result = userService.GetUsersByFilter(model.SearchCriteria, searchText);
                }

                switch (result.Status)
                {
                    case ResultEnum.Success:
                        IEnumerable<ApplicationUser> pagedList = result.Data.ToPagedList<ApplicationUser>(page, 10);

                        model.PagedList = pagedList;
                        model.PagedUsers = GetPagedUsers(pagedList);

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

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Search(SearchUserViewModel model)
        {
            if (!ModelState.IsValid || String.IsNullOrEmpty(model.SearchText))
            {
                return RedirectToAction("Search", "Membership");
            }

            return RedirectToAction("Search", "Membership",
                new { filter = model.SearchCriteria.ToString(), searchText = model.SearchText });
        }

        // Convert the ApplicationUsers to PagedUserViewModels and assign role properties
        private IEnumerable<PagedUserViewModel> GetPagedUsers(IEnumerable<ApplicationUser> pagedList)
        {
            IEnumerable<PagedUserViewModel> pagedUsers = pagedList.ToPagedUserViewModels();

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

            return pagedUsers;
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

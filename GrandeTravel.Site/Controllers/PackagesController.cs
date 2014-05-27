using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Models;
using GrandeTravel.Site.Models.Packages;
using GrandeTravel.Site.Helpers.Mappers;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;
using System.Web.Security;
using GrandeTravel.Site.Models.Payment;
using GrandeTravel.Site.Helpers;

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles = "ActiveUser")]
    public class PackagesController : Controller
    {
        // Fields
        private IPackageService packageService;

        // Constructors
        public PackagesController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Package> repository = RepositoryFactory.GetRepository<Package>(unitOfWork);

            IManager<Package> packageManager = ManagerFactory.GetManager(repository);

            this.packageService = ServiceFactory.GetPackageService(packageManager);
        }

        #region Add Package

        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        [HttpGet]
        public ActionResult Add()
        {
            PackagesViewModel model = new PackagesViewModel();

            if (MvcApplication.ShowSampleFormData)
            {
                // Show dummy user data for model
                model = SampleModelData.GetSamplePackagesViewModel();
            }

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(PackagesViewModel model)
        {
            if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ErrorMessage", "The image field is required.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Process image
                string[] validImageTypes = new string[]
                {
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };

                if (!validImageTypes.Contains(model.ImageUpload.ContentType))
                {
                    ModelState.AddModelError("ErrorMessage", "Please choose either a GIF, JPG or PNG image.");
                    return View(model);
                }

                string uploadDir = @"~/Images/Packages";
                string imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                string imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                model.ImageUpload.SaveAs(imagePath);

                Package package = model.ToPackage();
                package.ImageUrl = imageUrl;
                package.Status = PackageStatusEnum.Available;
                package.ApplicationUserId = WebSecurity.CurrentUserId;

                Result<Package> result = packageService.AddPackage(package);

                if (result.Status == ResultEnum.Success)
                {
                    return RedirectToAction("Add", "Activities", new { packageId = result.Data.PackageId });
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", "Sorry, we were unable to create your package.");
                    model.DisableSubmit = true;
                    return View(model);
                }
            }

            ModelState.AddModelError("ErrorMessage", "Sorry, we were unable to create your package.");
            model.DisableSubmit = true;
            return View(model);
        }

        #endregion

        #region Edit Package

        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int packageId = id.GetValueOrDefault();

            PackagesViewModel model = new PackagesViewModel();

            Result<Package> packageResult = new Result<Package>();
            packageResult = packageService.GetPackageById(packageId);

            if (packageResult.Status == ResultEnum.Success && packageResult.Data.Status == PackageStatusEnum.Available)
            {
                model = packageResult.Data.ToPackagesViewModel();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(PackagesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Result<Package> packageResult = new Result<Package>();
                packageResult = packageService.GetPackageById(model.Id);

                Package package = new Package();

                if (packageResult.Status == ResultEnum.Success && packageResult.Data.Status == PackageStatusEnum.Available)
                {
                    package = packageResult.Data;

                    package.City = model.City;
                    package.State = model.State;
                    package.Accomodation = model.Accomodation;
                    package.Amount = model.Price;
                }
                else
                {
                    model.ErrorMessage = "Unable to edit the package.";
                    return View(model);
                }

                ResultEnum result = packageService.UpdatePackage(package);

                if (result == ResultEnum.Success)
                {
                    model.SuccessMessage = "Package successfully edited.";
                }
                else
                {
                    model.ErrorMessage = "Unable to edit the package.";
                }

                return View(model);
            }

            model.ErrorMessage = "Unable to edit the package.";
            return View(model);
        }

        #endregion

        #region Search Packages For Customer

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Search()
        {
            SearchPackagesViewModel model = new SearchPackagesViewModel();

            Result<IEnumerable<Package>> result = packageService.GetAllPackages(false);

            switch (result.Status)
            {
                case ResultEnum.Success:
                    model.Packages = result.Data.ToList<Package>();
                    break;

                case ResultEnum.Fail:
                    model.Packages = null;
                    break;

                default:
                    model.Packages = null;
                    break;
            }

            return View(model);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Search(int packageId)
        {
            // User wants to book a package.
            // Redirect to the Register page if the user is not registered,
            // or straight to the payment page if already registered as a customer.

            try
            {
                if (Roles.IsUserInRole("Provider") || Roles.IsUserInRole("Admin") || !Roles.IsUserInRole("ActiveUser"))
                {
                    return RedirectToAction("Index", "Home");
                }

                // TODO : Redirect Anonymous to Register Page then to Checkout
                if (!Roles.IsUserInRole("Customer"))
                {
                    return RedirectToAction("Add", "Membership");
                }

                return RedirectToAction("CreateTransaction", "Payment", new { packageId = packageId });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // AJAX Get Package details
        private class AjaxPackage
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Accomodation { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetPackageDetails(int id)
        {
            Result<Package> result = packageService.GetPackageById(id);

            var packageArray = new[] { new AjaxPackage {
                Accomodation = result.Data.Accomodation,
                City = result.Data.City,
                State = result.Data.State.ToString(),
                Price = result.Data.Amount,
                ImageUrl = result.Data.ImageUrl
            }
        };
            return Json(packageArray, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search Packages For Provider

        [HttpGet]
        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult ProviderSearch()
        {
            SearchProviderPackagesViewModel model = new SearchProviderPackagesViewModel();

            int providerId = WebSecurity.CurrentUserId;
            Result<IEnumerable<Package>> result = packageService.GetPackagesByProviderId(providerId);

            switch (result.Status)
            {
                case ResultEnum.Success:
                    model.Packages = result.Data.ToList<Package>();
                    break;

                case ResultEnum.Fail:
                    break;

                default:
                    break;
            }

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProviderSearch(int packageId)
        {
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Discontinue Package

        [HttpPost]
        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        public JsonResult Discontinue(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.DenyGet);
            }

            int packageId = id.GetValueOrDefault();

            ResultEnum result;
            try
            {
                result = packageService.DiscontinuePackage(packageId);
                if (result == ResultEnum.Success)
                {
                    return Json(new { success = true, packageId = packageId }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.DenyGet);
            }
        }
        #endregion

        #region Show Map

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ShowMap()
        {
            return View();
        }

        #endregion
    }
}

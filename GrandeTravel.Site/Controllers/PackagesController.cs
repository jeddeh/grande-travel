using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GrandeTravel.Entity;
using GrandeTravel.Site.Models;
using System.Web.Routing;
using GrandeTravel.Service;
using GrandeTravel.Data;
using GrandeTravel.Manager;
using GrandeTravel.Site.Models.Packages;
using GrandeTravel.Entity.Enums;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    public class PackagesController : Controller
    {
        // Fields
        private IPackageService packageService;

        // Constructors
        public PackagesController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Package> repository = RepositoryFactory.GetRepository<Package>(unitOfWork);

            IPackageManager packageManager = ManagerFactory.GetPackageManager(repository);

            this.packageService = ServiceFactory.GetPackageService(packageManager);
        }

        #region Add Package

        [Authorize(Roles = "Provider")]
        [HttpGet]
        public ActionResult Add()
        {
            // Dummy user data for model
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);

            AddPackagesViewModel model = new AddPackagesViewModel
            {
                PackageName = "Package " + randomNumber,
                Price = 900.00m,
                City = "Melbourne",
                State = AustralianStateEnum.VIC,
                Accomodation = "5 nights at the Grand Hotel, Melbourne"
            };

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        [HttpPost]
        public ActionResult Add(AddPackagesViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Add", "Activities");
            }
            // validation errors disable submit button allow back only
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
        [HttpPost]
        public ActionResult Search(int packageId)
        {
            return RedirectToAction("Index", "Home");
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

        public JsonResult GetPackageDetails(int id)
        {
            Result<Package> result = packageService.GetPackageById(id);

            var packageArray = new[] { new AjaxPackage {
                Accomodation = result.Data.Accomodation,
                City = result.Data.City,
                State = result.Data.State.ToString(),
                Price = result.Data.Price,
                ImageUrl = result.Data.ImageUrl
            }
        };
            return Json(packageArray, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search Packages For Provider

        [HttpGet]
        [Authorize(Roles = "Provider")]
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
        [HttpPost]
        public ActionResult ProviderSearch(int packageId)
        {
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Discontinue Package

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public JsonResult Discontinue(int? id)
        {
            // TODO : Security concern where providers can discontinue other provider's packages?
            if (id == null)
            {
                return Json(new { success = false },JsonRequestBehavior.DenyGet);
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
    }
}

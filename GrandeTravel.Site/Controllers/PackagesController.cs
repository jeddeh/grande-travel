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

namespace GrandeTravel.Site.Controllers
{
    [AllowAnonymous]
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

        // Get All Packages
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
                //StartDate = result.Data.StartDate != null ?  result.Data.StartDate.Value.ToString("dd MMMM yyyy") : "",
                //EndDate = result.Data.EndDate != null ?  result.Data.EndDate.Value.ToString("dd MMMM yyyy") : "",
                Accomodation = result.Data.Accomodation,
                City = result.Data.City,
                State = result.Data.State.ToString(),
                //Description = result.Data.Description,
                Price = result.Data.Price,
                ImageUrl = result.Data.ImageUrl
            }
        };
            return Json(packageArray, JsonRequestBehavior.AllowGet);
        }
    }
}

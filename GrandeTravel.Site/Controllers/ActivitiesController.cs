using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrandeTravel.Site.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        // Fields
        private IActivityService activityService;
        private IPackageService packageService;

        // Constructors
        public ActivitiesController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Activity> activityRepository = RepositoryFactory.GetRepository<Activity>(unitOfWork);
            IActivityManager activityManager = ManagerFactory.GetActivityManager(activityRepository);
            this.activityService = ServiceFactory.GetActivityService(activityManager);

            IRepository<Package> packageRepository = RepositoryFactory.GetRepository<Package>(unitOfWork);
            IPackageManager packageManager = ManagerFactory.GetPackageManager(packageRepository);
            this.packageService = ServiceFactory.GetPackageService(packageManager);
        }
        #region Add Activity

        [Authorize (Roles = "Provider")]
        public ActionResult Add(int packageId)
        {
            Result<Package> result = new Result<Package>();
            result = packageService.GetPackageById(packageId);

            AddActivitiesViewModel model = new AddActivitiesViewModel();

            if (result.Status == ResultEnum.Success)
            {
                model.PackageId = result.Data.PackageId;
                model.PackageName = result.Data.Name;
                model.ActivityNumber = result.Data.Activities.Count + 1;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize (Roles = "Provider")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(AddActivitiesViewModel model)
        {
            Activity activity = new Activity()
            {
                Name = model.ActivityName,
                Description = model.Description,
                Address = model.Address,
                Status = PackageStatusEnum.Available,
                PackageId = model.PackageId
            };

            ResultEnum result = activityService.AddActivity(activity);

            if (result == ResultEnum.Success)
            {
                model.SuccessMessage = "The activity has been added to your package.";
            }
            else
            {
                model.ErrorMessage = "Unable to add the activity to the package.";
            }

            return View(model);
        }

        #endregion

        #region Discontinue Activity

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public JsonResult Discontinue(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.DenyGet);
            }

            int activityId = id.GetValueOrDefault();

            ResultEnum result;
            try
            {
                result = activityService.DiscontinueActivity(activityId);
                if (result == ResultEnum.Success)
                {
                    return Json(new { success = true, activityId = activityId }, JsonRequestBehavior.DenyGet);
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
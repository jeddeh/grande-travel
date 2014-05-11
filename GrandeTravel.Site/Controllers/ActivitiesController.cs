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
            IManager<Activity> activityManager = ManagerFactory.GetManager(activityRepository);
            this.activityService = ServiceFactory.GetActivityService(activityManager);

            IRepository<Package> packageRepository = RepositoryFactory.GetRepository<Package>(unitOfWork);
            IManager<Package> packageManager = ManagerFactory.GetManager(packageRepository);
            this.packageService = ServiceFactory.GetPackageService(packageManager);
        }

        #region Add Activity

        [Authorize(Roles = "Provider")]
        public ActionResult Add(int packageId)
        {
            Result<Package> result = new Result<Package>();
            result = packageService.GetPackageById(packageId);

            ActivitiesViewModel model = new ActivitiesViewModel();

            if (result.Status == ResultEnum.Success && result.Data.Activities.Count < 3)
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

        [Authorize(Roles = "Provider")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(ActivitiesViewModel model)
        {
            if (ModelState.IsValid)
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

            model.ErrorMessage = "Unable to add the activity to the package.";
            return View(model);
        }

        #endregion

        #region Edit Activity

        [Authorize(Roles = "Provider")]
        public ActionResult Edit(int? id, int? pId)
        {
            if (id == null || pId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int activityId = id.GetValueOrDefault();
            int packageId = pId.GetValueOrDefault();

            ActivitiesViewModel model = new ActivitiesViewModel();
            
            // Get name of package
            Result<Package> packageResult = new Result<Package>();
            packageResult = packageService.GetPackageById(packageId);

            if (packageResult.Status == ResultEnum.Success && packageResult.Data.Status == PackageStatusEnum.Available)
            {
                model.PackageId = packageId;
                model.PackageName = packageResult.Data.Name;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            // Get activity details
            Result<Activity> activityResult = new Result<Activity>();
            activityResult = activityService.GetActivityById(activityId);

            if (activityResult.Status == ResultEnum.Success && activityResult.Data.Status == PackageStatusEnum.Available)
            {
                model.ActivityName = activityResult.Data.Name;
                model.Address = activityResult.Data.Address;
                model.Description = activityResult.Data.Description;
                model.ActivityId = activityResult.Data.ActivityId;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize(Roles = "Provider")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(ActivitiesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Activity activity = new Activity()
                {
                    ActivityId = model.ActivityId,
                    Name = model.ActivityName,
                    Description = model.Description,
                    Address = model.Address,
                    Status = PackageStatusEnum.Available,
                    PackageId = model.PackageId
                };

                ResultEnum result = activityService.UpdateActivity(activity);

                if (result == ResultEnum.Success)
                {
                    model.SuccessMessage = "Activity successfully edited.";
                }
                else
                {
                    model.ErrorMessage = "Unable to edit the activity.";
                }

                return View(model);
            }

            model.ErrorMessage = "Unable to edit the activity.";
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
using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using GrandeTravel.Site.Models;
using GrandeTravel.Site.Models.Activities;
using GrandeTravel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace GrandeTravel.Site.Controllers
{
    [Authorize(Roles = "ActiveUser")]
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
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Add(int PackageId)
        {
            ActivitiesViewModel model = new ActivitiesViewModel();

            Result<Package> result = new Result<Package>();
            result = packageService.GetPackageById(PackageId);

            if (result.Status == ResultEnum.Success &&
                WebSecurity.CurrentUserId == result.Data.ApplicationUserId &&
                result.Data.Activities.Count < MvcApplication.MAX_ACTIVITIES)
            {
                model.PackageId = result.Data.PackageId;
                model.PackageName = result.Data.Name;
                model.PackageCity = result.Data.City;
                model.PackageState = result.Data.State;
                model.ActivityNumber = result.Data.Activities.Count + 1;
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
        public ActionResult Add(ActivitiesViewModel model)
        {
            string errorMessage = "Unable to add the activity to the package.";
            string successMessage = "The activity has been added to your package.";

            if (ModelState.IsValid)
            {
                try
                {
                    // Get co-ordianates of address
                    IGeolocationService geolocationService = UtilityFactory.GetGeolocationService();
                    Location location = geolocationService.GetCoordinates(
                        String.Format("{0}, {1}, {2}", model.Address, model.PackageCity, model.PackageState.ToString()));

                    Activity activity = new Activity()
                    {
                        Name = model.ActivityName,
                        Description = model.Description,
                        Address = model.Address,
                        Status = PackageStatusEnum.Available,
                        PackageId = model.PackageId,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    ResultEnum result = activityService.AddActivity(activity);

                    if (result == ResultEnum.Success)
                    {
                        model.SuccessMessage = successMessage;
                    }
                    else
                    {
                        model.ErrorMessage = errorMessage;
                    }

                    return View(model);
                }
                catch
                {
                    model.ErrorMessage = errorMessage;
                }
            }

            return View(model);
        }

        #endregion

        #region Edit Activity

        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
        public ActionResult Edit(int? ActivityId, int? PackageId)
        {
            if (ActivityId == null || PackageId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int activityId = ActivityId.GetValueOrDefault();
            int packageId = PackageId.GetValueOrDefault();

            ActivitiesViewModel model = new ActivitiesViewModel();

            // Get name of package
            Result<Package> packageResult = new Result<Package>();
            packageResult = packageService.GetPackageById(packageId);

            if (packageResult.Status == ResultEnum.Success && packageResult.Data.Status == PackageStatusEnum.Available)
            {
                model.PackageId = packageId;
                model.PackageName = packageResult.Data.Name;
                model.PackageCity = packageResult.Data.City;
                model.PackageState = packageResult.Data.State;
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
        [Authorize(Roles = "ActiveUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(ActivitiesViewModel model)
        {
            if (ModelState.IsValid)
            {
                string successMessage = "Activity successfully edited.";
                string errorMessage = "Unable to edit the activity.";

                try
                {
                    // Get co-ordianates of address
                    IGeolocationService geolocationService = UtilityFactory.GetGeolocationService();
                    Location location = geolocationService.GetCoordinates(
                        String.Format("{0}, {1}, {2}", model.Address, model.PackageCity, model.PackageState.ToString()));

                    Activity activity = new Activity()
                    {
                        ActivityId = model.ActivityId,
                        Name = model.ActivityName,
                        Description = model.Description,
                        Address = model.Address,
                        Status = PackageStatusEnum.Available,
                        PackageId = model.PackageId,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    ResultEnum result = activityService.UpdateActivity(activity);

                    if (result == ResultEnum.Success)
                    {
                        model.SuccessMessage = successMessage;
                    }
                    else
                    {
                        model.ErrorMessage = errorMessage;
                    }

                    return View(model);
                }
                catch
                {
                    model.ErrorMessage = errorMessage;
                }
            }

            return View(model);
        }

        #endregion

        #region Discontinue Activity

        [HttpPost]
        [Authorize(Roles = "Provider")]
        [Authorize(Roles = "ActiveUser")]
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
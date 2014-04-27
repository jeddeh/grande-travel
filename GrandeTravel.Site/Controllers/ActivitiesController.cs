using GrandeTravel.Data;
using GrandeTravel.Entity;
using GrandeTravel.Manager;
using GrandeTravel.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrandeTravel.Site.Controllers
{
    public class ActivitiesController : Controller
    {
        // Fields
        private IActivityService activityService;

        // Constructors
        public ActivitiesController()
        {
            IUnitOfWork unitOfWork = RepositoryFactory.GetUnitOfWork("DefaultConnection");

            IRepository<Activity> repository = RepositoryFactory.GetRepository<Activity>(unitOfWork);

            IActivityManager activityManager = ManagerFactory.GetActivityManager(repository);

            this.activityService = ServiceFactory.GetActivityService(activityManager);
        }

        public ActionResult Add()
        {
            return View();
        }

        #region Discontinue Activity

        [HttpPost]
        [Authorize(Roles = "Provider")]
        public JsonResult Discontinue(int? id)
        {
            // TODO : Security concern where providers can discontinue other provider's activities?
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
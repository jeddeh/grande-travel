using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;
using System.Data.Entity;

namespace GrandeTravel.Service.Implementation
{
    internal class ActivityService : IActivityService
    {
        // Fields
        private IActivityManager manager;

        // Constructors
        public ActivityService(IActivityManager manager)
        {
            this.manager = manager;
        }

        // Methods

        #region Add Activity to Package

        public ResultEnum AddActivity(Activity activity)
        {
            ResultEnum result;

            try 
            {
                manager.Create(activity);
                result = ResultEnum.Success;
            }
            catch 
            {
                result = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get Activity By Id

        public Result<Activity> GetActivityById(int id)
        {
            Result<Activity> result = new Result<Activity>();

            try
            {
                result.Data = manager.GetById(id);
                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Discontinue Activity

        public ResultEnum DiscontinueActivity(int activityId)
        {
            try
            {
                Activity activity = GetActivityById(activityId).Data;
                activity.Status = PackageStatusEnum.Discontinued;
                manager.Update(activity);
                return ResultEnum.Success;
            }
            catch
            {
                return ResultEnum.Fail;
            }
        }

        #endregion
    }
}

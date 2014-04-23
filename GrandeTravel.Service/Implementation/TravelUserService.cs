using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;
using GrandeTravel.Manager;

namespace GrandeTravel.Service.Implementation
{
    internal class TravelUserService : ITravelUserService
    {
        // Fields
        private ITravelUserManager manager;

        // Constructors
        public TravelUserService(ITravelUserManager manager)
        {
            this.manager = manager;
        }

        // Methods

        #region Create Travel User

        public ResultEnum CreateTravelUser(TravelUser travelUser)
        {
            ResultEnum result;

            try
            {
                manager.Create(travelUser);
                result = ResultEnum.Success;
            }
            catch (Exception)
            {
                result = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get Travel User By Id

        public Result<TravelUser> GetTravelUserById(int id)
        {
            Result<TravelUser> result = new Result<TravelUser>();

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

        #region Update Travel User

        public ResultEnum UpdateTravelUser(TravelUser travelUser)
        {
            ResultEnum result;

            try
            {
                manager.Update(travelUser);
                result = ResultEnum.Success;
            }
            catch (Exception)
            {
                result = ResultEnum.Fail;
            }

            return result;
        }

        #endregion
    }
}

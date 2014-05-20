using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;
using GrandeTravel.Manager;

namespace GrandeTravel.Service.Implementation
{
    internal class ApplicationUserService : IApplicationUserService
    {
        // Fields
        private IManager<ApplicationUser> manager;

        // Constructors
        public ApplicationUserService(IManager<ApplicationUser> manager)
        {
            this.manager = manager;
        }

        // Methods

        #region Create Travel User

        public ResultEnum CreateApplicationUser(ApplicationUser ApplicationUser)
        {
            ResultEnum result;

            try
            {
                manager.Create(ApplicationUser);
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

        public Result<ApplicationUser> GetApplicationUserById(int id)
        {
            Result<ApplicationUser> result = new Result<ApplicationUser>();

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

        #region Get All Users

        public Result<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            Result<IEnumerable<ApplicationUser>> result = new Result<IEnumerable<ApplicationUser>>();

            try
            {
                result.Data = manager.Get(p => true);
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

        public ResultEnum UpdateApplicationUser(ApplicationUser ApplicationUser)
        {
            ResultEnum result;

            try
            {
                manager.Update(ApplicationUser);
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

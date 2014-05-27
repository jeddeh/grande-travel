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
    internal class PackageService : IPackageService
    {
        // Fields
        private IManager<Package> manager;

        // Constructors
        public PackageService(IManager<Package> manager)
        {
            this.manager = manager;
        }

        // Methods
        #region Add Package

        public Result<Package> AddPackage(Package package)
        {
            Result<Package> result = new Result<Package>();

            try
            {
                result.Data = manager.Create(package);
                result.Status = ResultEnum.Success;
            }
            catch
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Update Package

        public ResultEnum UpdatePackage(Package package)
        {
            try
            {
                manager.Update(package);
                return ResultEnum.Success;
            }
            catch
            {
                return ResultEnum.Fail;
            }
        }

        #endregion

        #region Get Package By Id

        public Result<Package> GetPackageById(int id)
        {
            Result<Package> result = new Result<Package>();
            try
            {
                result.Data = manager.EagerGet((p => p.PackageId == id), "Activities").First();
                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get All Packages

        public Result<IEnumerable<Package>> GetAllPackages(bool getDiscontinuedPackages)
        {
            Result<IEnumerable<Package>> result = new Result<IEnumerable<Package>>();

            try
            {
                if (getDiscontinuedPackages)
                {
                    result.Data = manager.Get(p => true);
                }
                else
                {
                    result.Data = manager.Get(p => p.Status == PackageStatusEnum.Available).AsEnumerable<Package>();
                }
                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Get Packages By ProviderId

        public Result<IEnumerable<Package>> GetPackagesByProviderId(int providerId)
        {
            Result<IEnumerable<Package>> result = new Result<IEnumerable<Package>>();

            try
            {
                result.Data = manager.EagerGet(p => p.ApplicationUserId == providerId, "Activities")
                .AsEnumerable<Package>();

                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion

        #region Discontinue Package

        public ResultEnum DiscontinuePackage(int packageId)
        {
            try
            {
                Package package = GetPackageById(packageId).Data;
                package.Status = PackageStatusEnum.Discontinued;
                manager.Update(package);
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

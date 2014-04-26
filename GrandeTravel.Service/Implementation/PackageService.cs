﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;
using GrandeTravel.Manager;

namespace GrandeTravel.Service.Implementation
{
    internal class PackageService : IPackageService
    {
        // Fields
        private IPackageManager manager;

        // Constructors
        public PackageService(IPackageManager manager)
        {
            this.manager = manager;
        }

        // Methods

        #region Get Package By Id

        public Result<Package> GetPackageById(int id)
        {
            Result<Package> result = new Result<Package>();

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
                    result.Data = manager.Get(p => true).Where(p => p.Status == PackageStatusEnum.Available).AsEnumerable<Package>();
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
                result.Data = manager.Get(p => true).Where(p => p.TravelUserId == providerId).AsEnumerable<Package>();
                result.Status = ResultEnum.Success;
            }
            catch (Exception)
            {
                result.Status = ResultEnum.Fail;
            }

            return result;
        }

        #endregion
    }
}
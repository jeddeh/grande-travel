using GrandeTravel.Entity;
using GrandeTravel.Site.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Mappers
{
    public static class PackageMapper
    {
        public static PackagesViewModel ToPackagesViewModel(this Package package)
        {
            return new PackagesViewModel
            {
                PackageId = package.PackageId,
                PackageName = package.Name,
                City = package.City,
                State = package.State,
                Accomodation = package.Accomodation,
                ImageUrl = package.ImageUrl,
                Price = package.Price
            };
        }

        public static Package ToPackage(this PackagesViewModel model)
        {
            return new Package
            {
                Accomodation = model.Accomodation,
                City = model.City,
                ImageUrl = model.ImageUrl,
                Name = model.PackageName,
                Price = model.Price,
                State = model.State,
            };
        }
    }
}
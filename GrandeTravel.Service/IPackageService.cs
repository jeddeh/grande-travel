using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;

namespace GrandeTravel.Service
{
    public interface IPackageService
    {
        Result<Package> GetPackageById(int id);
        Result<IEnumerable<Package>> GetAllPackages(bool getDiscontinuedPackages);
        Result<IEnumerable<Package>> GetPackagesByProviderId(int providerId);
        ResultEnum DiscontinuePackage(int packageId);
        Result<Package> AddPackage(Package package);
        ResultEnum UpdatePackage(Package package);
    }
}

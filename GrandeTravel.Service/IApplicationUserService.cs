using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;

namespace GrandeTravel.Service
{
    public interface IApplicationUserService
    {
        ResultEnum CreateApplicationUser(ApplicationUser ApplicationUser);
        Result<ApplicationUser> GetApplicationUserById(int id);
        ResultEnum UpdateApplicationUser(ApplicationUser ApplicationUser);
    }
}

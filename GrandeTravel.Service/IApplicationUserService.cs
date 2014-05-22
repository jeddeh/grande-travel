using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Service
{
    public interface IApplicationUserService
    {
        ResultEnum CreateApplicationUser(ApplicationUser ApplicationUser);
        Result<ApplicationUser> GetApplicationUserById(int id);
        Result<IEnumerable<ApplicationUser>> GetAllUsers();
        Result<IEnumerable<ApplicationUser>> GetUsersByFilter(SearchUserEnum filter, string searchText);
        ResultEnum UpdateApplicationUser(ApplicationUser ApplicationUser);
    }
}

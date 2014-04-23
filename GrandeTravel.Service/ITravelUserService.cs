using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;

namespace GrandeTravel.Service
{
    public interface ITravelUserService
    {
        ResultEnum CreateTravelUser(TravelUser travelUser);
        Result<TravelUser> GetTravelUserById(int id);
        ResultEnum UpdateTravelUser(TravelUser travelUser);
    }
}

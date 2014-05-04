using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandeTravel.Entity;

namespace GrandeTravel.Service
{
    public interface IActivityService
    {
        Result<Activity> GetActivityById(int id);
        ResultEnum DiscontinueActivity(int packageId);
        ResultEnum AddActivity(Activity activity);
        ResultEnum UpdateActivity(Activity activity);
    }
}

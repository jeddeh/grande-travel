using GrandeTravel.Entity;
using GrandeTravel.Site.Models.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Helpers.Mappers
{
    public static class LocationMapper
    {
        public static ShowMapViewModel ToShowMapViewModel(this IEnumerable<Activity> activities)
        {
            ShowMapViewModel model = new ShowMapViewModel();
            model.Locations = new List<LocationModel>();

            foreach (Activity activity in activities)
            {
                if (activity.Latitude != 0 && activity.Longitude != 0)
                {
                    model.Locations.Add(new LocationModel
                    { 
                        ActivityName = activity.Name,
                        ActivityAddress = activity.Address,
                        Latitude = activity.Latitude,
                        Longitude = activity.Longitude
                    });
                }
            }

            return model;
        }
    }
}
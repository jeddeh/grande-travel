using GrandeTravel.Entity;

using System;
using System.Linq;

namespace GrandeTravel.Utility.Implementation
{
    internal class GoogleGeolocationService : IGeolocationService
    {
        // http://stackoverflow.com/questions/7942095/google-maps-v3-geocoding-server-side?lq=1
        public Location GetCoordinates(string address)
        {
            Location location = new Location();

            try
            {
                string requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false",
                   Uri.EscapeDataString(address));

                dynamic googleResults = new Uri(requestUri).GetDynamicJsonObject();
                // var resultsType = googleResults.results.GetType();

                foreach (var result in googleResults.results)
                {
                    // var locationType = result.geometry.location.lat.GetType();
                    location.Latitude = result.geometry.location.lat;
                    location.Longitude = result.geometry.location.lng;
                }
            }
            catch
            {
                // Request failed
            }

            return location;
        }
    }
}
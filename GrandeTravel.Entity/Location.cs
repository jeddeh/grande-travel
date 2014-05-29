using System;

namespace GrandeTravel.Entity
{
    // This class should not be persisted to the database.
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

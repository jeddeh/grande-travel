using System;
using GrandeTravel.Entity;

namespace GrandeTravel.Utility
{
    public interface IGeolocationService
    {
        Location GetCoordinates(string address);
    }
}

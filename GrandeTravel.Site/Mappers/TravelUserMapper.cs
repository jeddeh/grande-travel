using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrandeTravel.Entity;
using GrandeTravel.Site.Models;

namespace GrandeTravel.Site.Mappers
{
    public static class TravelUserMapper
    {
        public static T ToMembershipViewModel<T>(this TravelUser travelUser) where T :  MembershipViewModel, new()
        {
            MembershipViewModel model = new T
            {
                Address = travelUser.Address,
                City = travelUser.City,
                Email = travelUser.Email,
                FirstName = travelUser.FirstName,
                LastName = travelUser.LastName,
                Phone = travelUser.Phone,
                Postcode = travelUser.Postcode,
                State = travelUser.State
            };

            return (T)model;
        }
    }
}

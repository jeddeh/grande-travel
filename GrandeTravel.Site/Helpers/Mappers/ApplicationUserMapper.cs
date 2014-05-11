using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrandeTravel.Entity;
using GrandeTravel.Site.Models;

namespace GrandeTravel.Site.Mappers
{
    public static class ApplicationUserMapper
    {
        public static T ToMembershipViewModel<T>(this ApplicationUser ApplicationUser) where T :  MembershipViewModel, new()
        {
            MembershipViewModel model = new T
            {
                Address = ApplicationUser.Address,
                City = ApplicationUser.City,
                Email = ApplicationUser.Email,
                FirstName = ApplicationUser.FirstName,
                LastName = ApplicationUser.LastName,
                Phone = ApplicationUser.Phone,
                Postcode = ApplicationUser.Postcode,
                State = ApplicationUser.State
            };

            return (T)model;
        }

        public static ApplicationUser ToApplicationUser(this MembershipViewModel model)
        {
            return new ApplicationUser
            {
                Address = model.Address,
                City = model.City,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                Postcode = model.Postcode,
                State = model.State
            };
        }
    }
}

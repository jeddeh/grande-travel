using GrandeTravel.Entity;
using GrandeTravel.Site.Models.Membership;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Helpers.Mappers
{
    public static class PagedUsersMapper
    {
        public static IEnumerable<PagedUserViewModel> ToPagedUserViewModels(this IEnumerable<ApplicationUser> users)
        {
            List<PagedUserViewModel> models = new List<PagedUserViewModel>();

            foreach (ApplicationUser user in users)
            {
                PagedUserViewModel model = new PagedUserViewModel
                {
                    ApplicationUserId = user.ApplicationUserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };

                models.Add(model);
            };

            return models;
        }
    }
}
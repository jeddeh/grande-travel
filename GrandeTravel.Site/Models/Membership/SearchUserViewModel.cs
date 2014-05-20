using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrandeTravel.Site.Models.Membership
{
    public class SearchUserViewModel
    {
        [EnumDataType(typeof(SearchUserEnum))]
        [Display(Name = "Search By")]
        public SearchUserEnum SearchCriteria { get; set; }

        public IEnumerable<ApplicationUser> PagedList { get; set; }

        public IEnumerable<PagedUserViewModel> PagedUsers { get; set; }
    }
}

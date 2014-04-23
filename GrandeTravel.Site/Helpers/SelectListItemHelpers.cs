using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Helpers
{
    public static class SelectListItemHelpers
    {
        public static IEnumerable<SelectListItem> GetAustralianStateSelectListItems()
        {
            IEnumerable<AustralianStateEnum> values = Enum.GetValues(typeof(AustralianStateEnum)).Cast<AustralianStateEnum>();
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = value.ToString(),
                                                    Value = value.ToString()
                                                };
            return items;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GrandeTravel.Site.Helpers
{
    public static class SelectListItemHelpers
    {
        // Converts an Enum to an IEnumerable of SelectListItems for use with dropdown menus
        public static IEnumerable<SelectListItem> GetEnumSelectListItems<T>()
        {
            IEnumerable<T> values = Enum.GetValues(typeof(T)).Cast<T>();
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

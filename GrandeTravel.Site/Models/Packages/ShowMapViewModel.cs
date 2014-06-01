using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models.Packages
{
    public class ShowMapViewModel
    {
        [Required]
        public List<LocationModel> Locations { get; set; }
    }
}

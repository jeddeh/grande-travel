using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models.Packages
{
    public class LocationModel
    {
        [Required]
        public string ActivityName { get; set; }

        [Required]
        public string ActivityAddress { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
    }
}
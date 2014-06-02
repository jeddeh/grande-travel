using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models.Orders
{
    public class FeedbackViewModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string PackageName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Feedback { get; set; }
    }
}
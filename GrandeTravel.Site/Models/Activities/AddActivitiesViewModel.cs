﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models
{
    public class AddActivitiesViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Activity Name", Prompt = "Activity Name")]
        [DataType(DataType.Text)]
        public string ActivityName { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Description", Prompt = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Address", Prompt = "Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models.Packages
{
    public class AddPackagesViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Package Name", Prompt = "Package Name")]
        [DataType(DataType.Text)]
        public string PackageName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "City", Prompt = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [EnumDataType(typeof(AustralianStateEnum))]
        [Display(Name = "State")]
        public AustralianStateEnum State { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Accomodation", Prompt = "Accomodation Details")]
        [DataType(DataType.Text)]
        public string Accomodation { get; set; }

        // Image
        
        [Required]
        [RegularExpression(@"^\d{2,4}(\.\d{1,2})?$", ErrorMessage = "The Currency field is not valid.")]
        [Display(Name = "Price", Prompt = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
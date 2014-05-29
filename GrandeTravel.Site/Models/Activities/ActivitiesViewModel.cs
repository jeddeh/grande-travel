using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models.Activities
{
    public class ActivitiesViewModel
    {
        [Required]
        public int PackageId { get; set; }

        [Required]
        public string PackageName { get; set; }

        [Required]
        public string PackageCity { get; set; }

        [Required]
        [EnumDataType(typeof(AustralianStateEnum))]
        public AustralianStateEnum PackageState { get; set; }

        [Required]
        public int ActivityId { get; set; }
        
        [Required]
        public int ActivityNumber { get; set; }

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

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }
    }
}
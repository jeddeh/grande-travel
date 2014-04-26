using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models
{
    public class RegisterUserViewModel : MembershipViewModel
    {
        // Required password fields - used for creating new user accounts
        [Required]
        [MaxLength(50)]
        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The Password field must be at least 6 characters long.")]
        public override string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password fields do not match.")]
        public override string ConfirmPassword { get; set; }
    }
}

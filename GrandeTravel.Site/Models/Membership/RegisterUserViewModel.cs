using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models.Membership
{
    public class RegisterUserViewModel : MembershipViewModel
    {
        // Required password fields - used for creating new user accounts
        [Required]
        [MaxLength(50)]
        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,50}$", ErrorMessage = "Must be at least 8 characters and include a number, capital and lowercase letter.")]
        public override string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password fields do not match.")]
        public override string ConfirmPassword { get; set; }

        // Used when an Admin creates a new user account.
        public bool AccountCreatedSuccessfully { get; set; }

        // Used when the user wants to order a package
        public bool HasPackage { get; set; }

        public int PackageId { get; set; }
    }
}

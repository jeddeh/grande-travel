﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models.Membership
{
    public class EditUserViewModel : MembershipViewModel
    {
        // Not [Required] password fields - used for editing user accounts
        [MaxLength(50)]
        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(.{6,})?$", ErrorMessage = "The Password field must be at least 6 characters long.")]
        public override string Password { get; set; }

        [MaxLength(50)]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password fields do not match.")]
        public override string ConfirmPassword { get; set; }

        [Required]
        public bool IsAdminEdit { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsInactiveUser { get; set; }
    }
}

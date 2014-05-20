using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrandeTravel.Entity;

namespace GrandeTravel.Site.Models.Membership
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email", Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        [MinLength(6, ErrorMessage = "The email or password is incorrect.")]
        public string Password { get; set; }
    }
}

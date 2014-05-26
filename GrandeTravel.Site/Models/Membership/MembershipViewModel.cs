using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using GrandeTravel.Entity;
using GrandeTravel.Entity.Enums;

namespace GrandeTravel.Site.Models.Membership
{
    public abstract class MembershipViewModel
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public abstract string Password { get; set; }
        public abstract string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name", Prompt = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Address", Prompt = "Address")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

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
        [Display(Name = "Postcode", Prompt = "Postcode")]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "The Postcode field is invalid.")]
        [DataType(DataType.PostalCode)]
        public string Postcode { get; set; }

        [Required]
        [MaxLength(15)]
        [Phone]
        [Display(Name = "Phone", Prompt = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public bool IsProvider { get; set; }

        public bool IsAdmin { get; set; }
    }
}

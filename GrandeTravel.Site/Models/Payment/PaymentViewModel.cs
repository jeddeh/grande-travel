using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models.Payment
{
    public class PaymentViewModel
    {
        [Required]
        [Display(Name = "Card Number", Prompt = "Card Number")]
        [DataType(DataType.CreditCard)]
        public string CCNumber { get; set; }

        [Required]
        [Display(Name = "CVV", Prompt = "CVV")]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Expiration Month", Prompt = "Expiration Month")]
        public string ExpirationMonth { get; set; }

        [Required]
        [Display(Name = "Expiration Year", Prompt = "Expiration Year")]
        public string ExpirationYear { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        public decimal Amount { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Display(Name = "Package Name")]
        public string PackageName { get; set; }
    }
}
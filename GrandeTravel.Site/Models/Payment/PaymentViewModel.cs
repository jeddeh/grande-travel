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
        [DataType(DataType.CreditCard)]
        public string CCNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("")]
        public string CCV { get; set; }

        [Required]
        public string ExpirationMonth { get; set; }

        [Required]
        public string ExpirationYear { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
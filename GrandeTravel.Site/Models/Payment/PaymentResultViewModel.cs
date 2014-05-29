using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models.Payment
{
    public class PaymentResultViewModel
    {
        [Required]
        public bool IsSuccess { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
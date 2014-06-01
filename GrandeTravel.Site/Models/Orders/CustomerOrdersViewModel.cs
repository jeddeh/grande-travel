using GrandeTravel.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrandeTravel.Site.Models.Orders
{
    public class CustomerOrdersViewModel
    {
        [Required]
        public List<Order> Orders { get; set; }
    }
}
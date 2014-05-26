using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Entity
{
    // This class is not configured in the DbContext and will not be persisted to the database.
    public class Payment
    {
        public string CCNumber { get; set; }
        public string CVV { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public decimal Amount { get; set; }
    }
}

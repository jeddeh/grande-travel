using System;

namespace GrandeTravel.Entity
{
    // This class should not be persisted to the database.
    public class Payment
    {
        public string CCNumber { get; set; }
        public string CVV { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public decimal Amount { get; set; }

        public int PackageId { get; set; }
        public string PackageName { get; set; }
    }
}

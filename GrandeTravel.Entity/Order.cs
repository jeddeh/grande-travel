using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrandeTravel.Entity
{
    public class Order
    {
        // Properties
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateBooked { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.MultilineText)]
        public string Feedback { get; set; }

        [Required]
        public bool Paid { get; set; }

        [DataType(DataType.Text)]
        public string TransactionId { get; set; }

        public int VoucherCode { get; set; }

        public int PackageId { get; set; }
        public virtual Package Package { get; set; }

        public int CustomerId { get; set; }
        public virtual ApplicationUser Customer { get; set; }
    }
}

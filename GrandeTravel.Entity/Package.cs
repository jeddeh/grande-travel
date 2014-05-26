using GrandeTravel.Entity.Enums;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrandeTravel.Entity
{
    public class Package
    {
        // Properties
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        [Index("PackageNameIndex", IsUnique=true)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [EnumDataType(typeof(AustralianStateEnum))]
        public AustralianStateEnum State { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string Accomodation { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(PackageStatusEnum))]
        public PackageStatusEnum Status { get; set; }

        public ICollection<Order> Orders { get; set; }

        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<Activity> Activities { get; set; }

        // Constructors
        public Package()
        {
            Orders = new List<Order>();
            Activities = new List<Activity>();
        }
    }
}

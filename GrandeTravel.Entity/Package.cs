using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using GrandeTravel.Entity.Enums;

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
        public decimal Price { get; set; }

        [Required]
        [EnumDataType(typeof(PackageStatusEnum))]
        public PackageStatusEnum Status { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public int TravelUserId { get; set; }
        public virtual TravelUser TravelUser { get; set; }

        public ICollection<Activity> Activities { get; set; }

        // Constructors
        public Package()
        {
            Bookings = new List<Booking>();
            Activities = new List<Activity>();
        }
    }
}

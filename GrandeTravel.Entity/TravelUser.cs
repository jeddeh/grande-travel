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
    public class TravelUser
    {
        // Properties
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TravelUserId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [EnumDataType(typeof(AustralianStateEnum))]
        public AustralianStateEnum State { get; set; }

        [Required]
        [MaxLength(4)]
        [RegularExpression(@"^[0-9]{4}$")]
        [DataType(DataType.PostalCode)]
        public string Postcode { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Package> Packages { get; set; }

        // Constructors
        public TravelUser()
        {
            Bookings = new List<Booking>();
        }
    }
}

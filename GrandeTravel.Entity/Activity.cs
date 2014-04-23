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
    public class Activity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        //[Required]
        //[DataType(DataType.MultilineText)]
        //public string Address { get; set; }

        //[Required]
        //[DataType(DataType.Text)]
        //public string City { get; set; }

        //[Required]
        //[EnumDataType(typeof(AustralianStateEnum))]
        //public AustralianStateEnum State { get; set; }

        //[Required]
        //[DataType(DataType.PostalCode)]
        //public string Postcode { get; set; }

        //[Required]
        //[DataType(DataType.ImageUrl)]
        //public string ImageUrl { get; set; }

        public int PackageId { get; set; }
        public virtual Package Package { get; set; }
    }
}

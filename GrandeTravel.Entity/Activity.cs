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
        [MaxLength(50)]
        [DataType(DataType.Text)]
        [Index("ActivityNameIndex", IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required]
        [EnumDataType(typeof(PackageStatusEnum))]
        public PackageStatusEnum Status { get; set; }

        public int PackageId { get; set; }
        public virtual Package Package { get; set; }
    }
}

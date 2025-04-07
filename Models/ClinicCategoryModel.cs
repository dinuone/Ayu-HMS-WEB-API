using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class ClinicCategoryModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column("name", TypeName = "varchar(128)")]
        public string ClinicCategoryName { get; set; }

        [Column("dayOfTheWeek", TypeName = "varchar(128)")]
        public string DayOfTheWeek { get; set; }

        [Column("description", TypeName = "varchar(256)")]
        public string Description { get; set; }
    }
}

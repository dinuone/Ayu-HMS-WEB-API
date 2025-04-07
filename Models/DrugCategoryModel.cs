using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class DrugCategoryModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugCategoryId { get; set; }
        [Required]
        [Column("Drugcategory", TypeName = "varchar(128)")]
        public string DrugCategory { get; set; }

        [Column ("description", TypeName ="varchar(256)")]
        public string Description { get; set; }
    }
}

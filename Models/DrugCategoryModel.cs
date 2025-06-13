using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("drug_category")]
    public class DrugCategoryModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name", TypeName = "varchar(128)")]
        public required string Name { get; set; }

        [Column ("description", TypeName ="varchar(256)")]
        public string? Description { get; set; }
    }
}

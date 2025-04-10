using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("disease_codes")]
    public class DiseaseCodeModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("disease_code", TypeName = "varchar(128)")]
        public required string DiseaseCode { get; set; }
       
        [Column("disease_name", TypeName = "varchar(128)")]
        public required string DiseaseName { get; set; }
        
        [Column("description", TypeName = "varchar(256)")]
        public string? Description { get; set; }

    }
}

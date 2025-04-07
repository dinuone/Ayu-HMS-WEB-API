using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class DiseaseCodeModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int DiseaseCodeId { get; set; }

        [Required]
        [Column("Diseasecode", TypeName = "varchar(128)")]
        public string DiseaseCode { get; set; }
        [Required]
        [Column("Diseasename", TypeName = "varchar(128)")]
        public string DiseaseName { get; set; }
        [Column("description", TypeName = "varchar(256)")]
        public string Description { get; set; }

    }
}

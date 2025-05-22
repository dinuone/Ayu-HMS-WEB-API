using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("treatment")]
    public class TreatmentModel : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(128)")]
        public required string Name { get; set; }

        [Column("price")]
        public decimal TreatmentPrice { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("treatment_category_id")]
        public long TreatmentCategoryId { get; set; }
    }
}

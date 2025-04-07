using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookInventory.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    public class TreatmentModel : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public required string TreatmentName { get; set; }

        [Column("price")]
        public decimal TreatmentPrice { get; set; }

        [Column("treatment_category_id")]
        public long TreatmentCategoryId { get; set; }
    }
}

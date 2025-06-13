using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("rates")]
    public class RatesModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("hospital_charge", TypeName ="decimal(16,2)")]
        public decimal HospitalCharge { get; set; }

        [Column("custom_charges", TypeName = "json")]
        public string? CustomCharges { get; set; }
    }
}

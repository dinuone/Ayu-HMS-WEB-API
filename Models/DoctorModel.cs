using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("doctor")]
    public class DoctorModel: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("speacialty", TypeName = "varchar(128)")]
        public string Specialty { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("doctor_fee", TypeName ="decimal(16,2)")]
        public decimal DoctorFee { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("patient_visits")]
    public class PatientVisitModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        
        [Column("visit_date")]
        public DateTime VisitDate { get; set; }
       
        [Column("patient_id")]
        public long PatientId { get; set; }

        [Column("patient_name")]
        public required string PatientName { get; set; }

        [Column("clinic_category_id")]
        public int ClinicCategoryId { get; set; }

        [Column("clinic_category_name", TypeName = "varchar(128)")]
        public required string ClinicCategoryName { get; set; }

        [Column("chit_number", TypeName = "varchar(64)")]
        public required string ChitNumber { get; set; }

        [Column("status", TypeName = "varchar(64)")]
        public required string Status { get; set; }

        [Column("remarks", TypeName = "varchar(256)")]
        public string? Remarks { get; set; }

        [Column("doctor_id")]
        public long DoctorId { get; set; }

        [Column("doctor_name")]
        public string DoctorName { get; set; }  

    }
}

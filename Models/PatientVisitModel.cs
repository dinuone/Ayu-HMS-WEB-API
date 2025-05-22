using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

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

        [Column("visit_type")]
        public PatientVisitTypes VisitType { get; set; }
       
        [Column("patient_id")]
        public long PatientId { get; set; }

        [Column("clinic_category_id")]
        public int ClinicCategoryId { get; set; }

        [Column("treatments")]
        public List<TreatmentData> Treatment { get; set; }    

        [Column("chit_number", TypeName = "varchar(64)")]
        public  string ChitNumber { get; set; }

        [Column("status", TypeName = "varchar(64)")]
        public required string Status { get; set; }

        [Column("remarks", TypeName = "varchar(256)")]
        public string? Remarks { get; set; }

        [Column("doctor_id")]
        public long DoctorId { get; set; }

        [Column("patient_type")]
        public PatientType PatientType { get; set; }

        [Column("feelo_app_ref")]
        public string? FeeloAppRef { get; set; }

    }
}

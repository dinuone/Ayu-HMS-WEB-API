using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Innofactor.EfCoreJsonValueConverter;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Models
{
    [Table("doctor_assigns")]
    public class DoctorAssginModel : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("doctor_id")]
        public long DoctorId { get; set; }

        [JsonField, Column("clinic_categories", TypeName = "json")]
        public List<ClinicData> ClinicCategories { get; set; }

        [JsonField, Column("treatments", TypeName = "json")]
        public List<TreatmentData> Treatments { get; set; }

        [Column("assigned_unit")]
        public AssignUnit AssignedUnit { get; set; }

        [JsonField, Column("days_and_shift", TypeName = "json")]
        public List<DaysAndShift> DaysAndShift { get; set; }
    }

 
}

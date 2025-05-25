using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DoctorAssignDTO
{
    public class DoctorAssignDataDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("doctor_id")]
        public long DoctorId { get; set; }

        [JsonPropertyName("clinic_categories")]
        public List<ClinicData> ClincCategories { get; set; }

        [JsonPropertyName("treatments")]
        public List<TreatmentData> Treatments { get; set; }

        [JsonPropertyName("assign_unit")]
        public string AssignUnit { get; set; }

        [JsonPropertyName("days_and_shift")]
        public List<DaysAndShift> DaysAndShifts { get; set; }
    }
}

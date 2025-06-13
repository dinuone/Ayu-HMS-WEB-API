using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using System.Text.Json.Serialization;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DoctorAssignDTO
{
    public class DoctorAssignListDTO : BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string? DoctorName { get; set; }

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

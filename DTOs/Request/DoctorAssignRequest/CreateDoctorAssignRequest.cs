using System.Text.Json.Serialization;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DoctorAssignRequest
{
    public class CreateDoctorAssignRequest
    {
        [JsonPropertyName("doctor_id")]
        public long DoctorId { get; set; }

        [JsonPropertyName("clinic_categories")]
        public List<long> ClinicCategories {  get; set; }

        [JsonPropertyName("treatments")]
        public List<long> Treatments {  get; set; }

        [JsonPropertyName("assign_unit")]
        public AssignUnit AssginUnit { get; set; }

        [JsonPropertyName("days_and_shift")]
        public List<DaysAndShift> DaysAndShift { get; set; }
    }


}

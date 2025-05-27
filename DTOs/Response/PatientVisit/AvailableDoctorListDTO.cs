
using System.Text.Json.Serialization;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisit
{
    public class AvailableDoctorListDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("doctor_name")]
        public string DoctorName { get; set; }

        [JsonPropertyName("doctor_fee")]
        public decimal DoctorFee { get; set; }

        [JsonPropertyName("available_days")]
        public List<DaysAndShift> AvailableDays { get; set; }
    }
}

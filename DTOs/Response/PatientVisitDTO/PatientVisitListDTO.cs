using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO
{
    public class PatientVisitListDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("patient_reg_no")]
        public string? PatientRegNo { get; set; }

        [JsonPropertyName("patient_name")]
        public string? PatientName { get; set; }

        [JsonPropertyName("nic_number")]
        public string? NicNumber { get; set; }

        [JsonPropertyName("visit_date")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("patient_type")]
        public string? PatientType { get; set; }

        [JsonPropertyName("visit_type")]
        public string? VisitType { get; set; }

        [JsonPropertyName("chit_number")]
        public string? ChitNumber { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("doctor_name")]
        public string? DoctorName { get; set; }
    }
}

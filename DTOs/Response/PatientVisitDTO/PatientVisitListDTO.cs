using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO
{
    public class PatientVisitListDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("patient_id")]
        public long PatientId { get; set; }

        [JsonPropertyName("patient_name")]
        public string PatientName { get; set; }

        [JsonPropertyName("visit_date")]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("clinic_category_id")]
        public int ClinicCategoryId { get; set; }

        [JsonPropertyName("clinic_category_name")]
        public string ClinicCategoryName { get; set; }

        [JsonPropertyName("chit_number")]
        public string ChitNumber { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("doctor_id")]
        public long DoctorId { get; set; }
        [JsonPropertyName("doctor_name")]
        public string? DoctorName { get; set; }
    }
}

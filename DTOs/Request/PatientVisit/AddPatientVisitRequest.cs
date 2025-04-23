using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit
{
    public class AddPatientVisitRequest
    {
        [JsonPropertyName("patient_id")]
        public long PatientId { get; set; }

        [JsonPropertyName("visit_date")]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("clinic_category_id")]
        public int ClinicCategoryId { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("visit_type")]
        public string? VisitType { get; set; }

        [JsonPropertyName("treatment_ids")]
        public List<long> TreatmentIds { get; set; }

        [JsonPropertyName("doctor_id")]
        public long DoctorId { get; set; }
    }
}

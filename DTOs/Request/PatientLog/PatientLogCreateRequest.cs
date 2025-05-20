using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog
{
    public class PatientLogCreateRequest
    {
        [JsonPropertyName("symptoms")]
        public string symptoms { get; set; }

        [JsonPropertyName("remarks")]
        public string remarks { get; set; }

        [JsonPropertyName("is_assigned_to_clinic")]
        public bool isAssignedToClinic { get; set; }

        [JsonPropertyName("next_visit_date")]
        public DateTime nextVisitDate { get; set; }

        [JsonPropertyName("prescription_id")]
        public long? PrescriptionId { get; set; }

        [JsonPropertyName("treatments_id")]
        public int? TreatmentsId { get; set; }

        [JsonPropertyName("clinic_id")]
        public int? ClinicId { get; set; }

        [JsonPropertyName("diagnosis_id")]
        public int? DiagnosisId { get; set; }

    }
}

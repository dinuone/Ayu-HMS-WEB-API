using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO
{
    public class PatientLogDataDTO
    {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("patient_id")]
        public long PatientId { get; set; }

        [JsonPropertyName("visit_id")]
        public long VisitId { get; set; }

        [JsonPropertyName("symptoms")]
        public string Symptoms { get; set; }

        [JsonPropertyName("is_assigned_to_clinic")]
        public bool IsAssignedToClinic { get; set; }

        [JsonPropertyName("next_visit_date")]
        public DateTime NextVisitDate { get; set; }

        [JsonPropertyName("remarks")]
        public string Remarks { get; set; }

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

using System.Text.Json.Serialization;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog
{
    public class PatientLogCreateRequest
    {
        [JsonPropertyName("patient_id")]
        public long PatientId { get; set; }

        [JsonPropertyName("patient_name")]
        public string PatientName { get; set; }

        [JsonPropertyName("visit_id")]
        public long VisitId { get; set; }

        [JsonPropertyName("visit_date")]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("doctor_id")]
        public long DoctorId { get; set; }

        [JsonPropertyName("doctor_name")]
        public string DoctorName { get; set; }

        [JsonPropertyName("symptoms")]
        public string symptoms { get; set; }

        [JsonPropertyName("remarks")]
        public string remarks { get; set; }

        [JsonPropertyName("is_assigned_to_clinic")]
        public bool isAssignedToClinic { get; set; }

        [JsonPropertyName("next_visit_date")]
        public DateTime nextVisitDate { get; set; }


        [JsonPropertyName("prescription")]
        public List<PrescriptionAttribute> Prescription { get; set; }

        [JsonPropertyName("treatments")]
        public List<TreatmentsAttribute> Treatments { get; set; }

        [JsonPropertyName("clinics")]
        public List<ClinicAttribute> Clinics { get; set; }

        [JsonPropertyName("diagnosis")]
        public List<DiagnosisAttribute> Diagnosis { get; set; }






    }
}

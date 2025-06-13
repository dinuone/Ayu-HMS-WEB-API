using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.InvoiceDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisit
{
    public class PatientVisitDataDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("patient_data")]
        public PatientDataDTO? PatientData { get; set; }

        [JsonPropertyName("visit_date")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("patient_type")]
        public string PatientType { get; set; }

        [JsonPropertyName("visit_type")]
        public string VisitType { get; set; }

        [JsonPropertyName("chit_number")]
        public string ChitNumber { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("doctor_name")]
        public string? DoctorName { get; set; }

        [JsonPropertyName("treatments")]
        public List<TreatmentData> Treatments { get; set; }

        [JsonPropertyName("clinic_category")]
        public string? ClinicCategory { get; set; }

        [JsonPropertyName("feelo_app_ref")]
        public string? FeeloAppRef { get; set; }

        [JsonPropertyName("invoice")]
        public InvoiceDataDTO Invoice {get;set;}
             

    }
}

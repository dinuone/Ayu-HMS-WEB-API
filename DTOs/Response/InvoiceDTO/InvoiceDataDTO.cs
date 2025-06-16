using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using System.Text.Json.Serialization;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.InvoiceDTO
{
    public class InvoiceDataDTO
    {
        [JsonPropertyName("invoice_no")]
        public string InvoiceNo { get; set; }

        [JsonPropertyName("patient_data")]
        public PatientDataDTO PatientData { get; set; }

        [JsonPropertyName("invoice_description")]
        public InvoiceDescription InvoiceDescription { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("sub_total")]
        public SubTotal SubTotal { get; set; }

        [JsonPropertyName("doctor_fee")]
        public decimal DoctorFee { get; set; }

        [JsonPropertyName("doctor_name")]
        public string DoctorName { get; set; }

        [JsonPropertyName("treatments")]
        public List<TreatmentsAttribute> Treatments { get; set; }

        [JsonPropertyName("clinics")]
        public List<ClinicAttribute> Clinics { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set; }

    }
}

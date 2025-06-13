using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO
{
    public class ClinicsDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("clinic_name")]
        public string ClinicName { get; set; }

    }
}

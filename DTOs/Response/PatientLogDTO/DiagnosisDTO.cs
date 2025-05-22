using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO
{
    public class DiagnosisDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("disease_code")]
        public string DiseaseCode { get; set; }
        [JsonPropertyName("disease_name")]
        public string DiseaseName { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}


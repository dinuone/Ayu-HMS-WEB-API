using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DiseaseCodeRequest
{
    public class DiseaseCodeRequest
    {
      
        [JsonPropertyName("disease_code")]
        public required string DiseaseCode { get; set; }
        
        [JsonPropertyName("disease_name")]
        public required string DiseaseName { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}

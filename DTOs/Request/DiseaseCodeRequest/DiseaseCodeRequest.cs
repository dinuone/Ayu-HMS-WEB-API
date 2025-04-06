using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DiseaseCodeRequest
{
    public class DiseaseCodeRequest
    {
        [Required]
        [JsonPropertyName("disease_code")]
        public string DiseaseCode { get; set; }
        
        [JsonPropertyName("disease_name")]
        public string DiseaseName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;
    }
}

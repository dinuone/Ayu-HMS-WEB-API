using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest
{
    public class DrugCategoryRequest
    {
        [Required]
        [JsonPropertyName("drug_category")]
        public string DrugCategory { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = true;
    }
}

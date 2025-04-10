using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentCategory
{
    public class AddTreatmentCategoryRequest
    {
        [JsonPropertyName("name")]
        public required string TreatmentName { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

    }
}

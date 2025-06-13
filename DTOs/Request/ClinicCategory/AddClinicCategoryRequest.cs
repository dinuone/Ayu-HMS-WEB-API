using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory
{
    public class AddClinicCategoryRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("available_days")]
        public required string AvailableDays { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

    }
}

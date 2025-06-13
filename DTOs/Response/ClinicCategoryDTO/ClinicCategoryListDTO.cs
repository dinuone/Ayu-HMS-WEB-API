using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO
{
    public class ClinicCategoryListDTO : BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? ClinicCategoryName { get; set; }

        [JsonPropertyName("available_days")]
        public string[]? AvailableDays { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

    }
}

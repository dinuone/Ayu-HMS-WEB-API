using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO
{
    public class TreatmentCategoryDataDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string TreatmentName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}

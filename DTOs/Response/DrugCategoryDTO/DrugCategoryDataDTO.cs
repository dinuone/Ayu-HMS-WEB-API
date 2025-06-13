using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO
{
    public class DrugCategoryDataDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string DrugCategoryName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}

using System.Text.Json.Serialization;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugDTO
{
    public class DrugDataDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string DrugName { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("drug_category")]
        public long DrugCategory { get; set; }


    }
}

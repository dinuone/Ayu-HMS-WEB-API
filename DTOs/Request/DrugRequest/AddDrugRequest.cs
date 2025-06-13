using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugRequest
{
    public class AddDrugRequest
    {
        [JsonPropertyName("name")]
        public required string DrugName { get; set; }

        [JsonPropertyName("drug_code")]
        public required string DrugCode { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("drug_category")]
        public int DrugCategoryId { get; set; }
       
    }
}

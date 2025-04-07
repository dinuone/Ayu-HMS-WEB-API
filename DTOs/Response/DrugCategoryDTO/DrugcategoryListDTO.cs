using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO
{
    public class DrugcategoryListDTO: BaseResponseDTO
    {
        [JsonPropertyName("DrugCategoryId")]
        public int DrugCategoryId { get; set; }
        [JsonPropertyName("DrugCategory")]
        public string DrugCategory { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
    }
   
}

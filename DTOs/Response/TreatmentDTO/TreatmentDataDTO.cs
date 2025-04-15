using System.Text.Json.Serialization;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO
{
    public class TreatmentDataDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("treatment_name")]
        public string TreatmentName { get; set; }

        [JsonPropertyName("treatment_price")]
        public decimal TreatmentPrice { get; set; }

        [JsonPropertyName("treatment_category")]
        public long TreatmentCategory { get; set; } 
    }
}

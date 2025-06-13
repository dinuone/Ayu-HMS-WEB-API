using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentRequest
{
    public class TreatmentCreateRequest
    {
        [JsonPropertyName("treatment_name")]
        public string TreatmentName { get; set; }

        [JsonPropertyName("treatment_price")]
        public decimal TreatmentPrice { get; set; }

        [JsonPropertyName("treatment_category")]
        public long TreatmentCategoryId { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}

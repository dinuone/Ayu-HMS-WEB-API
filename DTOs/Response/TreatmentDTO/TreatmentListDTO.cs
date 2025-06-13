using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO
{
    public class TreatmentListDTO : BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string TreatmentName { get; set; }

        [JsonPropertyName("price")]
        public decimal TreatmentPrice { get; set; }

        [JsonPropertyName("treatment_category")]
        public TreatmentCategoryDataDTO? TreatmentCategory { get; set; }
    }
}

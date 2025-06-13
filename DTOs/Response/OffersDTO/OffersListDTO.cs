using System.Text.Json.Serialization;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.OffersDTO
{
    public class OffersListDTO : BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("offer_name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("discount_percentage")]
        public decimal DiscountPercentage { get; set; }

        [JsonPropertyName("start_date")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("treatment")]
        public TreatmentDataDTO Treatment { get; set; }

        [JsonPropertyName("platform")]
        public string? Platform { get; set; }

    }
}

using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.OffersRequest
{
    public class AddOffersRequest
    {
        [JsonPropertyName("offer_name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("discount_percentage")]
        public decimal DiscountPercentage { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("treatment")]
        public int TreatmentId { get; set; }
    }
}

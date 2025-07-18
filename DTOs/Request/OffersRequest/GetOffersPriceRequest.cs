using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.OffersRequest
{
    public class GetOffersPriceRequest
    {
        [JsonPropertyName("treatment_ids")]
        public long[] TreatmentId { get; set; }
    }
}

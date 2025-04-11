using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatesRequest
{
    public class RatesUpdateRequest
    {
        [JsonPropertyName("hospital_charge")]
        public decimal HospitalCharge { get; set; }

        [JsonPropertyName("custom_charge")]
        public string? CustomCharge { get; set; }
    }
}

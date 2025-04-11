using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.RatesDTO
{
    public class RatesDataDTO
    {
        [JsonPropertyName("hospital_charge")]
        public decimal HospitalCharge { get; set; }

        [JsonPropertyName("custom_charge")]
        public string CustomCharge { get; set; }
    }
}

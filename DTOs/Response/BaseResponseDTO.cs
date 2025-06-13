using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response
{
    public class BaseResponseDTO
    {
        [JsonPropertyName("created_by")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set; }
    }
}

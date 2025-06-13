using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.User
{
    public class ChangePasswordRequest
    {
        [JsonPropertyName("current_password")]
        public required string CurrentPassword { get; set; }

        [JsonPropertyName("new_password")]
        public required string NewPassword { get; set; }
    }
}

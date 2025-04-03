using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth
{
    public class UserCreateRequest
    {
        [JsonPropertyName("username")]
        [Required]
        public string UserName { get; set; }

        [JsonPropertyName("first_name")]
        [Required]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        [Required]
        public string LastName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}

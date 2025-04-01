using BookInventory.Helpers.Utils;
using System.Text.Json.Serialization;

namespace BookInventory.DTOs.Response.User
{
    public class UserListDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirtstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime CreatedAt { get; set;}
        

    }
}

using System.Text.Json.Serialization;

namespace BookInventory.DTOs.Request.User
{
    public class AddRoleRequest
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}

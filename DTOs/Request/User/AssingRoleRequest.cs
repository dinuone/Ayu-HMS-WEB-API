using System.Text.Json.Serialization;

namespace BookInventory.DTOs.Request.User
{
    public class AssingRoleRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string RoleName { get; set; }
    }
}

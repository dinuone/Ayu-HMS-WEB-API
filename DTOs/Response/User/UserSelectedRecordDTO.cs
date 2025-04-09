using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User
{
    public class UserSelectedRecordDTO
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
        public RoleDataDTO? Role { get; set; }

        [JsonPropertyName("branch")]
        public List<BranchDataDTO> Branches { get; set; }

    }
}

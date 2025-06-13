using System.Text.Json.Serialization;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch
{
    public class BranchListDTO : BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get;set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; } 

    }
}

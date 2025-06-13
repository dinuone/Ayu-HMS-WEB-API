using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch
{
    public class BranchRequest
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

       
    }
}

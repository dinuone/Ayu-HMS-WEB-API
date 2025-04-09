using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch
{
    public class BranchFilterrequest
    {
        [JsonPropertyName("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonPropertyName("is_active")]
        public string IsActive { get; set; }  
    }
}

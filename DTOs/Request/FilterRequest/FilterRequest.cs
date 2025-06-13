using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest
{
    public class FilterRequest
    {
        [JsonPropertyName("from_date")]
        public DateTime? FromDate { get; set; }

        [JsonPropertyName("to_date")]
        public DateTime? ToDate { get; set; }

        [JsonPropertyName("is_active")]
        public string IsActive { get; set; }
    }

    public class FilterRequestClinic : FilterRequest
    {
        [JsonPropertyName("available_days")]
        public List<string>? AvailableDays { get; set; }
    }

    public class FilterRequestTreatment : FilterRequest
    {
        [JsonPropertyName("category_ids")]
        public int[] TreatmentCategoryIds { get; set; }
    }


    public class FilterRequestDrug : FilterRequest
    {
        [JsonPropertyName("category_ids")]
        public int[] DrugCategoryIds { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatingsRequest
{
    public class AddRatingRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("stars")]
        [Range(1, 5)]
        public int Stars { get; set; }
    }
}

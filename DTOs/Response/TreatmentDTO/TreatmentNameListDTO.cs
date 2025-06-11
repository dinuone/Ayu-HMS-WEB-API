using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO
{
    public class TreatmentNameListDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO
{
    public class DiseaseCodeListDTO: BaseResponseDTO
    {
        [JsonPropertyName("Diseasecodeid")]
        public int DiseaseCodeId { get; set; }
        [JsonPropertyName("Diseasecode")]
        public string DiseaseCode { get; set; }
        [JsonPropertyName("Diseasename")]
        public string DiseaseName { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DoctorDTO
{
    public class DoctorListDTO: BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("speacialty")]
        public string Specialty { get; set; }
        [JsonPropertyName("doctor_fee")]
        public decimal DoctorFee { get; set; }
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
 
}

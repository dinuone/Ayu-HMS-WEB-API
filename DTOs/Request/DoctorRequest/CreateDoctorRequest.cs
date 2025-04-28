using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DoctorRequest
{
    public class CreateDoctorRequest
    {
        [JsonPropertyName("speacialty")]
        public string Specialty { get; set; }

        [JsonPropertyName("doctor_fee")]
        public decimal DoctorFee { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}

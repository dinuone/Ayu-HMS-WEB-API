using System.Text.Json.Serialization;


namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientDTO
{
    public class PatientListDTO : BaseResponseDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("registration_number")]
        public required string RegistrationNumber { get; set; }

        [JsonPropertyName("age")]
        public required int Age { get; set; }

        [JsonPropertyName("gender")]
        public required string Gender { get; set; }

        [JsonPropertyName("nic_number")]
        public string NicNumber { get; set; }

        [JsonPropertyName("patient_type")]
        public string PatientType { get; set; }

        [JsonPropertyName("contact_no")]
        public string ContactNumber { get; set; }
      
    }
}

using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest
{
    public class AddPatientRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("registration_number")]
        public required string RegistrationNumber { get; set; }

        [JsonPropertyName("age")]
        public required int Age { get; set; }

        [JsonPropertyName("gender")]
        public required string Gender { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("marital_status")]
        public string MaritalStatus { get; set; }

        [JsonPropertyName("occupation")]
        public string Occupation { get; set; }

        [JsonPropertyName("contact_no")]
        public required string ContactNo { get; set; }

        [JsonPropertyName("patient_type")]
        public required string PatientType { get; set; }

        [JsonPropertyName("qr_code")]
        public string QrCode { get; set; }

        [JsonPropertyName("address_line_1")]
        public string AddressLine1 { get; set; }

        [JsonPropertyName("address_line_2")]
        public string AddressLine2 { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }



    }
}

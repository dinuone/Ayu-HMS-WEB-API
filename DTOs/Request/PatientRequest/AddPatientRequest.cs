using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest
{
    public class AddPatientRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("age")]
        public required int Age { get; set; }

        [JsonPropertyName("gender")]
        public required string Gender { get; set; }

        [JsonPropertyName("weight")]
        public required int Weight { get; set; }

        [JsonPropertyName("marital_status")]
        public required string MaritalStatus { get; set; }

        [JsonPropertyName("occupation")]
        public string? Occupation { get; set; }

        [JsonPropertyName("contact_no")]
        public required string ContactNo { get; set; }

        [JsonPropertyName("qr_code")]
        public required string QrCode { get; set; }

        [JsonPropertyName("address_line_1")]
        public string? AddressLine1 { get; set; }

        [JsonPropertyName("address_line_2")]
        public string? AddressLine2 { get; set; }

        [JsonPropertyName("city")]
        public required string City { get; set; }

        [JsonPropertyName("district")]
        public required string District { get; set; }

        [JsonPropertyName("province")]
        public required string Province { get; set; }

        [JsonPropertyName("nic_number")]
        public required string NicNumber { get; set; }   



    }
}

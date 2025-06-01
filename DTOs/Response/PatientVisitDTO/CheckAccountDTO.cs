using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO
{
    public class CheckAccountDTO
    {
        [JsonPropertyName("account_exisit")]
        public bool AccountExisit { get; set; }

        [JsonPropertyName("patient_reg_no")]
        public string PatientRegNo { get; set; }
    }
}

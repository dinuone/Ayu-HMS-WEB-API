using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit
{
    public class TreatmentIdsRequest
    {
        [JsonPropertyName("treatments")]
        public List<long> TreatmentIds { get; set; }
    }
}

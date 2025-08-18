using System.Text.Json.Serialization;
using Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO
{
    public class PatientVisitTimelineDTO
    {
        [JsonPropertyName("visit_id")]
        public long VisitId { get; set; }

        [JsonPropertyName("visit_date")]
        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime VisitDate { get; set; }

        [JsonPropertyName("doctor_name")]
        public string DoctorName { get; set; }

        [JsonPropertyName("visit_type")]
        public string VisitType { get; set; }

        [JsonPropertyName("treatments")]
        public List<string> Treatments { get; set; }

        [JsonPropertyName("clinics")]
        public List<string> Clinics { get; set; }

        [JsonPropertyName("chit_no")]
        public string ChitNo { get; set; }

        [JsonPropertyName("next_visit_date")]

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime NextVisitDate { get; set; }

    }
}

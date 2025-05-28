
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit
{
    public class AddPatientVisitRequest
    {
        [JsonPropertyName("patient_reg_no")]
        public string PatientRegNo { get; set; }

        [JsonPropertyName("clinic_category_id")]
        public int ClinicCategoryId { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("patient_type")]
        public PatientType PatientType {  get; set; }  

        [JsonPropertyName("visit_type")]
        public PatientVisitTypes VisitType { get; set; }

        [JsonPropertyName("treatments")]
        public List<TreatmentData> Treatments { get; set; }

        [JsonPropertyName("doctor_id")]
        public long DoctorId { get; set; }

        [JsonPropertyName("feelo_app_ref")]
        public string? FeeloAppRef { get; set; }
    }

    public class TreatmentData
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name {  get; set; }   
    }

    public class ClinicData
    {

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

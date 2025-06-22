using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest
{
    public class PatientImportFromExcelRequest
    {

        [JsonPropertyName("Patient_excel")]
        public required List<AddPatientRequest> Patients { get; set; }
    }
}

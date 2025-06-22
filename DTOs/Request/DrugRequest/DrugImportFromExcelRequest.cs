using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugRequest
{
    public class DrugImportFromExcelRequest
    {
        [JsonPropertyName("drug_excel")]
        public required List<AddDrugRequest> Drugs { get; set; }
    }
}

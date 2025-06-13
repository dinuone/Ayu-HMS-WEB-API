using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientLogDTO
{
    public class MasterDataDTO
    {
        public List<DiseaseCodeNameListDTO> DiseaseCodes { get; set; }
        public List<DrugNameListDTO> Drugs { get; set; }
        public List<TreatmentNameListDTO> Treatments { get; set; }
        public List<ClinicNameListDTO> Clinics { get; set; }
    }
}

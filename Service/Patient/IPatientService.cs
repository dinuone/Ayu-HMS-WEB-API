using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Patient
{
    public interface IPatientService
    {
        Task<BaseResponse> Create(AddPatientRequest request);
        Task<BaseResponse> Update(long id, AddPatientRequest request);
        Task<BaseResponse> GetById(long id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(long id);
        Task<BaseResponse> UpdateStatus(long id);
        Task<BaseResponse> DeleteAll(List<long> patientIds);
        Task<BaseResponse> Filter(FilterRequest request);
        Task<BaseResponse> ImportPatientsFromCsv(PatientImportFromExcelRequest request);
        Task<BaseResponse> GetPatientRecordForView(long id);

    }
}

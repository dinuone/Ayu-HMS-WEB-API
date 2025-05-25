using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientVisit
{
    public interface IPatientVisitService
    {
        Task<BaseResponse> Create(AddPatientVisitRequest request);
        Task<BaseResponse> Update(int id, AddPatientVisitRequest request);
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll(bool IsActive);
        Task<BaseResponse> Filter(FilterRequest request);
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> DeleteAll(List<int> PatientVisitIds);
        Task<BaseResponse> GetAvailableClinicsOrTreatments(PatientVisitTypes patientVisitType);
       

    }
}

using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.PatientLog
{
    public interface IPatientLogService
    {
        Task<BaseResponse> Create(PatientLogCreateRequest request);
        Task<BaseResponse> Update(long id, PatientLogCreateRequest request);
        Task<BaseResponse> GetById(long id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> GetMasterDataForPatientLog();
       
      
    }
}

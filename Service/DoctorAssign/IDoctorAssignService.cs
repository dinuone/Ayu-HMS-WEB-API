using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DoctorAssignRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.DoctorAssign
{
    public interface IDoctorAssignService
    {
        Task<BaseResponse> Create(CreateDoctorAssignRequest request);
        Task<BaseResponse> Update(int id, CreateDoctorAssignRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> DeleteAll(List<int> assignIds);
    }
}

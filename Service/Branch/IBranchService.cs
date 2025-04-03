
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Branch
{
    public interface IBranchService
    {
        Task<BaseResponse> Create(BranchRequest request);
        Task<BaseResponse> Update(int id, BranchRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(int id);
    }
}

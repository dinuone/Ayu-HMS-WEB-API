using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.User;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Auth
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
        Task<BaseResponse> Create(UserCreateRequest request);
        Task<BaseResponse> List();
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> Update(int id, UserUpdateRequest request);
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> DeleteAll(List<int> userIds);
        Task<BaseResponse> ChangePassword(ChangePasswordRequest request);
        Task<BaseResponse> GetRoleList();
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> GetBranchList();
        Task<BaseResponse> GetDoctorsList();


    }
}

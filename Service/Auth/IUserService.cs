using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
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
        Task<UserModel?> GetById(int id);

    }
}

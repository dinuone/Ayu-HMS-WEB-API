using BookInventory.DTOs.Request.Auth;
using BookInventory.DTOs.Request.User;
using BookInventory.DTOs.Response.User;
using BookInventory.Helpers;
using BookInventory.Models;

namespace BookInventory.Service.Auth
{
    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
        Task<BaseResponse> Create(UserCreateRequest request);
        Task<BaseResponse> List();
        Task<UserModel?> GetById(int id);

    }
}

using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.User;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AuthorizeAttribute = Harendra_Ayurveda_HMS_WEB_API.Helpers.AuthorizeAttribute;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
       
            BaseResponse response = await userService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            BaseResponse response = await userService.List();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetRecordFoEdit(int id)
        {
            BaseResponse response = await userService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await userService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        // Update User
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateRequest request)
        {
            // Call the service to update the user details
            BaseResponse response = await userService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        // Delete User (Soft Delete)
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Call the service to delete (soft delete) the user
            BaseResponse response = await userService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        // Delete User (Soft Delete)
        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) the user
            BaseResponse response = await userService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("role-list")]
        public async Task<IActionResult> RoleList()
        {
            BaseResponse response = await userService.GetRoleList();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("branch-list")]
        public async Task<IActionResult> BranchList()
        {
            BaseResponse response = await userService.GetBranchList();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

    }
}

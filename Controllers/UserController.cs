using BookInventory.DTOs.Request.Auth;
using BookInventory.DTOs.Request.User;
using BookInventory.Helpers;
using BookInventory.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthorizeAttribute = BookInventory.Helpers.AuthorizeAttribute;

namespace BookInventory.Controllers
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

       
     
    }
}

using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

       
     
    }
}

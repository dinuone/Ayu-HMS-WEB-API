using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService; 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {

            AuthenticateResponse response = await userService.Authenticate(request);

            if (response.GetMessage() != null)
            {
                var msg = response.GetMessage();

                var resp = new BaseResponse();
                resp.GenerateResponse(new { message = msg }, HttpStatusCode.BadRequest);

                return new JsonResult(resp) { StatusCode = 400 };
            }

           return response.ToActionResult();
        }
    }
}

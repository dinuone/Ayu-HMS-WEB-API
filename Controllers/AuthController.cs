using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.Controllers
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

            return response.ToActionResult();
        }
    }
}

using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // Attach user to HttpContext.User
                var response = await userService.GetById(userId);
                var userDto = response.Data as UserSelectedRecordDTO;
                if (userDto != null)
                {
                   var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userDto.Username),
                        new Claim("id", userDto.Id.ToString())
                        // Add more claims as necessary
                    };

                    var identity = new ClaimsIdentity(claims, "Bearer");
                    var principal = new ClaimsPrincipal(identity);

                    context.User = principal;

                    context.User = principal; // Set HttpContext.User
                }
            }
            catch
            {
                // Do nothing if JWT validation fails
                // User is not attached to context, so the request won't have access to secure routes
            }
        }
    }

}

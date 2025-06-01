using System.Security.Claims;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.UserContext
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long UserId => GetUserId();
        public string Username => _httpContextAccessor.HttpContext?.User.Identity?.Name;

        private long GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                throw new UnauthorizedAccessException("User ID claim is missing or invalid");
            }
            return userId;
        }
    }
}

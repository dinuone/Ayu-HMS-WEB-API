using System.Security.Claims;

namespace Harendra_Ayurveda_HMS_WEB_API.Middleware
{
    public class CreateByMiddleware
    {
        private readonly RequestDelegate _next;

        public CreateByMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var usernameClaim = context.User.FindFirst(ClaimTypes.Name);
                var userIdClaim = context.User.FindFirst("id");

                if (usernameClaim != null && userIdClaim != null)
                {
                    context.Items["CreatedBy"] = usernameClaim.Value;

                    // Parse to long before storing
                    if (long.TryParse(userIdClaim.Value, out long authId))
                    {
                        context.Items["authId"] = authId; // Now stored as long
                    }
                    else
                    {
                        // Handle invalid ID format
                        throw new UnauthorizedAccessException("Invalid user ID format");
                    }
                }
            }
            await _next(context);
        }
    }
}

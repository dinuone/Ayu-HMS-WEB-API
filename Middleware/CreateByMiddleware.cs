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
            // Check if the user is authenticated
            if (context.User.Identity.IsAuthenticated)
            {
                // Get the user ID from the claims (this should be set by your authentication provider)
                var usernameClaim = context.User.FindFirst(ClaimTypes.Name);

                if (usernameClaim != null)
                {
                    // Store the user ID (CreatedBy) in the HttpContext items
                    context.Items["CreatedBy"] = usernameClaim.Value;
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}

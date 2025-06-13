using System.ComponentModel.DataAnnotations;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

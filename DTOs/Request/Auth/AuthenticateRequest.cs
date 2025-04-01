using System.ComponentModel.DataAnnotations;

namespace BookInventory.DTOs.Request.Auth
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

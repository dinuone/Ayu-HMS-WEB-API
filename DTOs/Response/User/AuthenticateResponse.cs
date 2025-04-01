using BookInventory.Helpers;
using BookInventory.Models;
using System.Net;

namespace BookInventory.DTOs.Response.User
{
    public class AuthenticateResponse : BaseResponse
    {
        private string message;


        // constructors
        public AuthenticateResponse(string message)
        {
            this.message = message;
        }

        public AuthenticateResponse(UserModel user, string token, HttpStatusCode status)
        {
            var data = new
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Token = token,
            };

            GenerateResponse(data, status);
        }

        public string GetMessage()
        {
            return message;
        }
    }
}

using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using System.Net;

namespace Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User
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
                user.Email,
                user.FirstName,
                user.LastName,
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

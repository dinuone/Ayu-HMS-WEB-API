using BookInventory.DTOs.Request.Auth;
using BookInventory.DTOs.Request.User;
using BookInventory.DTOs.Response.User;
using BookInventory.Helpers;
using BookInventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using static BookInventory.Helpers.GlobalAttributeHelper;

namespace BookInventory.Service.Auth
{
    public class UserService : IUserService
    {
        private static Random random = new Random();
        private readonly ApplicationDBContext dBContext;
        private readonly ILogger<UserService> logger;
        TimeZoneInfo localTimeZone;
        private readonly IConfiguration configuration;

        public UserService(IConfiguration configuration, ApplicationDBContext dBContext, ILogger<UserService> logger)
        {
            this.dBContext = dBContext;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.configuration = configuration;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var user = await dBContext.Users.SingleOrDefaultAsync(x => x.Username == request.Username);

            if (user == null)
            {
                return new AuthenticateResponse("User Account Cannot Found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new AuthenticateResponse("Username or password is incorrect"); ;

            }

            // authentication successful so generate jwt token
            var token = await generateJwtToken(user);

            return new AuthenticateResponse(user, token, HttpStatusCode.OK);

        }

        public async Task<UserModel?> GetById(int id)
        {
            return await dBContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        private async Task<string> generateJwtToken(UserModel user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }

        public async Task<BaseResponse> Create(UserCreateRequest request)
        {

            BaseResponse response;
            logger.LogInformation("User Create :::::");
            try
            {

                UserModel? user = await dBContext.Users.FirstOrDefaultAsync(x => x.Username == request.UserName);

                if(user == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "username Already Exist"
                        }
                    };

                    return response;
                }

                //hashing the password
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                UserModel newUser = new UserModel()
                {
                    Username = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    isActive = true,
                    Password = hashPassword,
                    Role = request.Role
                };

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "User Account Created"
                    }
                };

                return response;


            }
            catch (Exception ex)
            {
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };

                logger.LogInformation("User Create Exception :::::" + ex.Message);

                return response;
            }

        }

    
        public async Task<BaseResponse>List()
        {
            BaseResponse response;
            try
            {
                var users = dBContext.Users.ToList();

                // Create a list to hold the DTOs
                List<UserListDTO> userList = [];

                foreach (var user in users)
                {
                  
                    // Map user to UserListDTO
                    var userDto = new UserListDTO
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role,
                        IsActive = user.isActive,
                        FirtstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedAt = user.CreatedAt
                    };

                    userList.Add(userDto);
                }

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = userList
                };

                return response;

            }
            catch (Exception ex)
            {
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };

                logger.LogInformation("User List Exception :::::" + ex.Message);

                return response;
            }
        }
    }
}

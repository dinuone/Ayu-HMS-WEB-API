using BookInventory.DTOs.Request.User;
using Harendra_Ayurveda_HMS_WEB_API;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.User;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
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
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Auth
{
    public class UserService : IUserService
    {
        private static Random random = new Random();
        private readonly ApplicationDBContext dBContext;
        private readonly ILogger<UserService> logger;
        TimeZoneInfo localTimeZone;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserService(IConfiguration configuration, ApplicationDBContext dBContext, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.dBContext = dBContext;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
   
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
            using var transaction = await dBContext.Database.BeginTransactionAsync();

            try
            {
                // Check if username already exists
                var existingUser = await dBContext.Users.FirstOrDefaultAsync(u => u.Username == request.UserName);
                if (existingUser != null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Username already exists" }
                    };
                }

                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                // Hash password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create and save user
                var newUser = new UserModel
                {
                    Username = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = hashedPassword,
                    Role = request.Role,
                    CreatedBy = createdBy,
                  
                };

                dBContext.Users.Add(newUser);
                await dBContext.SaveChangesAsync(); // Save first to get newUser.Id

                // Add user-branch relationships
                var userBranches = request.Branches.Select(branchId => new UserBranchModel
                {
                    UserId = newUser.Id,
                    BranchId = (int)branchId // assuming branchId is int
                });

                await dBContext.AddRangeAsync(userBranches);
                await dBContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "User account created successfully." }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError("User creation failed: " + ex.Message);
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "User creation failed." }
                };
            }
        }

        public async Task<BaseResponse> Update(int id, UserCreateRequest request)
        {
            try
            {
                var user = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);
                if (user == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "User not found" }
                    };
                }

                string? updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                user.Username = request.UserName;
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Role = request.Role;
                user.IsActive = request.IsActive;
                user.UpdatedBy = updatedBy;

                dBContext.Users.Update(user);
                await dBContext.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "User updated successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError("User update failed: " + ex.Message);
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "User update failed." }
                };
            }
        }

        public async Task<BaseResponse> Delete(int id)
        {
            try
            {
                var user = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);
                if (user == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "User not found" }
                    };
                }

                string? deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                user.IsDelete = true;
                user.UpdatedBy = deletedBy;

                dBContext.Users.Update(user);
                await dBContext.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "User deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError("User delete failed: " + ex.Message);
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "User delete failed." }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> userIds)
        {
            try
            {
                var users = await dBContext.Users
                    .Where(u => userIds.Contains(u.Id) && !u.IsDelete)
                    .ToListAsync();

                if (!users.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "No valid users found to delete." }
                    };
                }

                string? deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                foreach (var user in users)
                {
                    user.IsDelete = true;
                    user.UpdatedAt = DateTime.UtcNow;
                    user.UpdatedBy = deletedBy;
                }

                dBContext.Users.UpdateRange(users);
                await dBContext.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Users deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError("Bulk delete failed: " + ex.Message);
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "Bulk delete failed." }
                };
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
                        IsActive = user.IsActive,
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

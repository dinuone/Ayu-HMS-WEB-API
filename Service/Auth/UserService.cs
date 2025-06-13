using BookInventory.DTOs.Request.User;
using Harendra_Ayurveda_HMS_WEB_API;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Auth;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.User;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DoctorDTO;
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

            string role = dBContext.Roles.Where(r => r.Id == user.Role).Select(r => r.Name).FirstOrDefault();

            return new AuthenticateResponse(user, role, token, HttpStatusCode.OK);

        }

        public async Task<BaseResponse> GetById(int id)
        {
            BaseResponse response;
            try
            {
                var user = await (from u in dBContext.Users
                                  join r in dBContext.Roles on u.Role equals r.Id into roleGroup
                                  from role in roleGroup.DefaultIfEmpty()
                                  where u.IsDelete == false && u.Id == id
                                  select new UserSelectedRecordDTO
                                  {
                                      Id = u.Id,
                                      FirtstName = u.FirstName,
                                      LastName = u.LastName,
                                      Username = u.Username,
                                      Email = u.Email,
                                      Role = role != null ? new RoleDataDTO
                                      {
                                          Id = role.Id,
                                          Name = role.Name
                                      } : null,
                                      Branches = new List<BranchDataDTO>()
                                  }).FirstOrDefaultAsync();

                if (user != null)
                {
                    user.Branches = GetBranchList((int)user.Id, dBContext);
                }

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = user
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Get user record for edit Exception :::::" + ex.Message);

                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };
            }
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
                    Expires = DateTime.UtcNow.AddHours(3), // Set expiration to 3 hours
                    SigningCredentials = new SigningCredentials(
                          new SymmetricSecurityKey(key),
                          SecurityAlgorithms.HmacSha256Signature
                      )
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
                    IsInitialPassword = true,
                  
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

                if(request.Role == 3) //doctor
                {
                    var doctor = new DoctorModel
                    {
                        UserId = newUser.Id,
                        Specialty = request.Specialty,
                        DoctorFee = request.DoctorFee,
                        CreatedBy= createdBy,
                    };

                    dBContext.Doctors.Add(doctor);
                    await dBContext.SaveChangesAsync();
                }

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

        public async Task<BaseResponse> Update(int id, UserUpdateRequest request)
        {
            using var transaction = await dBContext.Database.BeginTransactionAsync();

            try
            {
                // Get the user being updated
                var user = await dBContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);
                if (user == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "User not found" }
                    };
                }

                // Check if the new username is different from current username
                if (user.Username != request.UserName)
                {
                    var usernameExists = await dBContext.Users
                        .AnyAsync(u => u.Username == request.UserName && u.Id != id && !u.IsDelete);

                    if (usernameExists)
                    {
                        return new BaseResponse
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Data = new { message = "Username already exists for another user." }
                        };
                    }
                }

                string? updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                // Update user properties
                user.Username = request.UserName;
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Role = request.Role;
                user.UpdatedBy = updatedBy;

                dBContext.Users.Update(user);
                await dBContext.SaveChangesAsync();

                // Update UserBranches
                var existingBranches = await dBContext.UserBranches.Where(ub => ub.UserId == id).ToListAsync();
                dBContext.UserBranches.RemoveRange(existingBranches);
                await dBContext.SaveChangesAsync();

                var updatedUserBranches = request.Branches.Select(branchId => new UserBranchModel
                {
                    UserId = id,
                    BranchId = (int)branchId
                });

                await dBContext.AddRangeAsync(updatedUserBranches);
                await dBContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "User updated successfully." }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
                var userList = await (from u in dBContext.Users
                                      join r in dBContext.Roles on u.Role equals r.Id into roleGroup
                                      from role in roleGroup.DefaultIfEmpty() where u.IsDelete == false
                                      select new UserListDTO
                                      {
                                          Id = u.Id,
                                          FirtstName = u.FirstName,
                                          LastName = u.LastName,
                                          Username = u.Username,
                                          Email = u.Email,
                                          Role = role != null ? role.Name : null,
                                          IsActive = u.IsActive,
                                          Branches = new List<BranchDataDTO>(),
                                          CreatedAt = u.CreatedAt
                                      }).ToListAsync();

                foreach (var user in userList)
                {
                    user.Branches = GetBranchList(user.Id, dBContext);
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

        public static List<BranchDataDTO> GetBranchList(long userId, ApplicationDBContext context)
        {

            List<BranchDataDTO> result = (from ub in context.UserBranches
                          join b in context.Branches on ub.BranchId equals b.Id
                          where ub.UserId == userId
                          select new BranchDataDTO
                          {
                              Id = b.Id,
                              Name = b.Name
                          }).ToList();

            return result;
        }

        //TODO
        public Task<BaseResponse> ChangePassword(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> GetRoleList()
        {
            BaseResponse response;
            try
            {
                List<RoleModel> roles = await dBContext.Roles.Where(r => r.Name != "SuperAdmin").ToListAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = roles
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

                logger.LogInformation("Role List Exception :::::" + ex.Message);

                return response;
            }
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                UserModel? user = await dBContext.Users.Where(u => u.IsDelete == false && u.Id == id).FirstOrDefaultAsync();
                
                if(user != null)
                {
                    user.IsActive = !user.IsActive;
                    dBContext.Users.Update(user);
                    await dBContext.SaveChangesAsync();
                }

                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"User status updated to {(user.IsActive ? "Active" : "Inactive")}." }
                };

                return response;

            }
            catch(Exception ex)
            {
                logger.LogError("User status update failed: " + ex.Message);
                response =  new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "User status update failed." }
                };

                return response;
            }
        }

        public  async Task<BaseResponse> GetBranchList()
        {
            BaseResponse response;
            try
            {
                List<BranchModel> branches = await dBContext.Branches.Where(b => b.IsDelete == false && b.IsActive == true).ToListAsync();

                List<BranchDataDTO> BranchListMapp = new List<BranchDataDTO>();
                foreach (var branch in branches)
                {
                    BranchListMapp.Add(new BranchDataDTO
                    {
                        Id = branch.Id,
                        Name = branch.Name,
                    });
                }

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = BranchListMapp
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
                return response;
            }
        }

        public async Task<BaseResponse> GetDoctorsList()
        {
            BaseResponse response;
            try
            {
                List<DoctorListDTO> doctors = await dBContext.Users.Where(u => u.Role == 3 && u.IsDelete == false && u.IsActive == true)
                    .Select(doctor => new DoctorListDTO
                    {
                        Id = doctor.Id,
                        Name = doctor.FirstName + " " + doctor.LastName,
                    })
                .ToListAsync();


                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = doctors
                };

                return response;

            }
            catch(Exception ex)
            {
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };
                return response;
            }
        }
    }
}

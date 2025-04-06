
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using System.Security.Claims;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Branch
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<BranchService> logger;
        TimeZoneInfo localTimeZone;

        public BranchService(ApplicationDBContext context, ILogger<BranchService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> Create(BranchRequest request)
        {
            try
            {
                var loggedInUser = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                if (loggedInUser == null)
                {
                    return BaseResponse.CreateResponse("User not logged in", HttpStatusCode.Unauthorized);
                }

                BranchModel branch = new BranchModel
                {
                    Name = request.Name,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    CreatedBy = loggedInUser,
                };

                _context.Set<BranchModel>().Add(branch);
                await _context.SaveChangesAsync();

                return BaseResponse.CreateResponse(branch, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating branch");
                return BaseResponse.CreateResponse("An error occurred while creating the branch", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse> Update(int id, BranchRequest request)
        {
            try
            {
                BranchModel? branch = await _context.Set<BranchModel>().FindAsync(id);
                if (branch == null)
                {
                    return BaseResponse.CreateResponse("Branch not found", HttpStatusCode.BadRequest);
                }

                branch.Name = request.Name;
                branch.Address = request.Address;
                branch.PhoneNumber = request.PhoneNumber;

                _context.Set<BranchModel>().Update(branch);
                await _context.SaveChangesAsync();

                return BaseResponse.CreateResponse(branch, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating branch");
                return BaseResponse.CreateResponse("An error occurred while updating the branch", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse> GetById(int id)
        {
            BaseResponse response;
            try
            {
                BranchModel? branch = await _context.Set<BranchModel>().FindAsync(id);
                if (branch == null)
                {

                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "Branch not found"
                        }
                    };

                    return response;
                }

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = branch
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
                logger.LogError(ex, "Error occurred while getting branch by id");
                return response;
            }
        }

        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;
            try
            {
                List<BranchModel> branches = await _context.Branches.Where(b => b.IsDelete == false).ToListAsync();

                List<BranchListDTO> BranchListMapp = new List<BranchListDTO>();
                foreach (var branch in branches)
                {
                    BranchListMapp.Add(new BranchListDTO
                    {
                        Id = branch.Id,
                        Name = branch.Name,
                        CreatedAt = branch.CreatedAt,
                        CreatedBy = branch.CreatedBy,
                        IsActive = branch.IsActive
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

        public async Task<BaseResponse> Delete(int id)
        {
            BranchModel? branch = await _context.Set<BranchModel>().FindAsync(id);
            if (branch == null)
            {
                return BaseResponse.CreateResponse("Branch not found", HttpStatusCode.NotFound);
            }

            branch.IsDelete = true;
            await _context.SaveChangesAsync();

            return BaseResponse.CreateResponse("Branch deleted", HttpStatusCode.OK);
        }
    }
}

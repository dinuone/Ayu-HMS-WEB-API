
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                    return BaseResponse.CreateResponse("User is not logged in.", HttpStatusCode.Unauthorized);
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

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Branch created successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating branch.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while creating the branch." }
                };
            }
        }

        public async Task<BaseResponse> Update(int id, BranchRequest request)
        {
            try
            {
                BranchModel? branch = await _context.Set<BranchModel>().FindAsync(id);
                if (branch == null)
                {
                    return BaseResponse.CreateResponse("Branch not found.", HttpStatusCode.BadRequest);
                }

                branch.Name = request.Name;
                branch.Address = request.Address;
                branch.PhoneNumber = request.PhoneNumber;

                _context.Set<BranchModel>().Update(branch);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Branch updated successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating branch.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating the branch." }
                };
            }
        }

        public async Task<BaseResponse> GetById(int id)
        {
            try
            {
                BranchModel? branch = await _context.Set<BranchModel>().FindAsync(id);
                if (branch == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Branch not found." }
                    };
                }

                var dto = new BranchListDTO
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    PhoneNumber = branch.PhoneNumber,
                    Address = branch.Address
                };

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving branch by ID.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while retrieving the branch." }
                };
            }
        }

        public async Task<BaseResponse> GetAll()
        {
            try
            {
                List<BranchModel> branches = await _context.Branches.Where(b => !b.IsDelete).ToListAsync();

                var branchList = branches.Select(branch => new BranchListDTO
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    PhoneNumber = branch.PhoneNumber,
                    Address = branch.Address,
                    CreatedAt = branch.CreatedAt,
                    CreatedBy = branch.CreatedBy,
                    IsActive = branch.IsActive
                }).ToList();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = branchList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all branches.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while retrieving branches." }
                };
            }
        }

        public async Task<BaseResponse> Delete(int id)
        {
            try
            {
                BranchModel? branch = await _context.Set<BranchModel>().FindAsync(id);

                if (branch == null || branch.IsDelete)
                {
                    return BaseResponse.CreateResponse("Branch not found.", HttpStatusCode.NotFound);
                }

                branch.IsDelete = true;

                var userBranches = await _context.UserBranches
                    .Where(ub => ub.BranchId == id)
                    .ToListAsync();

                _context.UserBranches.RemoveRange(userBranches);

                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Branch deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting branch.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting the branch." }
                };
            }
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            try
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);

                if (branch == null)
                {
                    return BaseResponse.CreateResponse("Branch not found.", HttpStatusCode.NotFound);
                }

                branch.IsActive = !branch.IsActive;
                _context.Branches.Update(branch);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"Branch status updated to {(branch.IsActive ? "Active" : "Inactive")}." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating branch status.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating branch status." }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> branchIds)
        {
            try
            {
                var branches = await _context.Branches
                    .Where(b => branchIds.Contains(b.Id) && !b.IsDelete)
                    .ToListAsync();

                if (!branches.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "No valid branches found to delete." }
                    };
                }

                string? deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                foreach (var branch in branches)
                {
                    if (branch.Id != 1)
                    {
                        branch.IsDelete = true;
                        branch.UpdatedBy = deletedBy;
                    }
                }

                var userBranchRecords = await _context.UserBranches
                    .Where(ub => branchIds.Contains(ub.Id))
                    .ToListAsync();

                _context.UserBranches.RemoveRange(userBranchRecords);
                _context.Branches.UpdateRange(branches);

                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Branches deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting multiple branches.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting branches." }
                };
            }
        }

        public async Task<BaseResponse> Filter(BranchFilterrequest request)
        {
            try
            {
                IQueryable<BranchModel> query = _context.Branches.Where(b => !b.IsDelete);

                if (request.FromDate != null)
                {
                    var fromDate = request.FromDate.Value.Date;
                    query = query.Where(sl => sl.CreatedAt >= fromDate);
                }

                if (request.ToDate != null)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                    query = query.Where(sl => sl.CreatedAt <= toDate);
                }

                if (request.IsActive != "All")
                {
                    if (request.IsActive == "active")
                        query = query.Where(sl => sl.IsActive);
                    else if (request.IsActive == "inactive")
                        query = query.Where(sl => !sl.IsActive);
                }

                var branchList = await query.Select(data => new BranchListDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    Address = data.Address,
                    IsActive = data.IsActive,
                    PhoneNumber = data.PhoneNumber,
                    CreatedAt = data.CreatedAt
                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = branchList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while filtering branches.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while filtering branches." }
                };
            }
        }
    }
}

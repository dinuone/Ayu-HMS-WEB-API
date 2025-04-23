using System.Net;
using System.Security.Claims;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DiseaseCodeRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode
{
    public class DiseaseCodeService : IDiseaseCodeService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<DiseaseCodeService> logger;
        TimeZoneInfo localTimeZone;

        public DiseaseCodeService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<DiseaseCodeService> logger)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> Create(DiseaseCodeRequest request)
        {
            BaseResponse response;
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                DiseaseCodeModel DiseaseCode = new DiseaseCodeModel
                {
                    DiseaseCode = request.DiseaseCode,
                    DiseaseName = request.DiseaseName,
                    Description = request.Description,
                    CreatedBy = createdBy,
                };
                _context.Set<DiseaseCodeModel>().Add(DiseaseCode);
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new {message = "Disease Code created successfully." }
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
                logger.LogError(ex, "An error occurred while creating new Disease code");
            }
            return response;
            
        }

        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                DiseaseCodeModel? DiseaseCode = await _context.Set<DiseaseCodeModel>().FindAsync(id);
                if (DiseaseCode == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Disease code not found"
                        }
                    };
                    return response;
                }

                DiseaseCode.IsDelete = true;
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Disease code deleted successfully"
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
                logger.LogError(ex, "Error occurred while deleting Disease code");
            }
            return response;
        }

        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;
            try
            {
                List<DiseaseCodeModel> DiseaseCodes = await _context.DiseaseCodes.Where(dc => dc.IsDelete == false).ToListAsync();
                List<DiseaseCodeListDTO> DiseaseCodeListMapp = new List<DiseaseCodeListDTO>();

                foreach (var DiseaseCode in DiseaseCodes)
                {
                    DiseaseCodeListMapp.Add(new DiseaseCodeListDTO
                    {
                        Id = DiseaseCode.Id,
                        DiseaseCode = DiseaseCode.DiseaseCode,
                        DiseaseName = DiseaseCode.DiseaseName,
                        Description = DiseaseCode.Description,
                        IsActive = DiseaseCode.IsActive,
                        CreatedBy = DiseaseCode.CreatedBy,
                        CreatedAt = DiseaseCode.CreatedAt
                    });

                }
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = DiseaseCodeListMapp
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving all disease codes");
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

        public async Task<BaseResponse> GetById(int id)
        {
            BaseResponse response;
            try
            {
                DiseaseCodeModel? DiseaseCode = await _context.Set<DiseaseCodeModel>().FindAsync(id);
                if (DiseaseCode == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Disease code not found"
                        }
                    };
                    return response;
                }

                var diseaseCodeDto = new DiseaseCodeDataDTO
                {
                    Id = DiseaseCode.Id,
                    DiseaseName = DiseaseCode.DiseaseName,
                    DiseaseCode = DiseaseCode.DiseaseCode,
                    Description = DiseaseCode.Description
                };

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = diseaseCodeDto
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
                logger.LogError(ex, "Error occurred while retrieving Disease code by ID");
            }
            return response;
        }

        public async Task<BaseResponse> Update(int id, DiseaseCodeRequest request)
        {
            BaseResponse response;
            try
            {
                DiseaseCodeModel? DiseaseCode = await _context.Set<DiseaseCodeModel>().FindAsync(id);
                if (DiseaseCode == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Disease code not found"
                        }
                    };
                    return response;
                }

                string? updateBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                DiseaseCode.DiseaseCode = request.DiseaseCode;
                DiseaseCode.DiseaseName = request.DiseaseName;
                DiseaseCode.Description = request.Description;
                DiseaseCode.UpdatedBy = updateBy;

                _context.Set<DiseaseCodeModel>().Update(DiseaseCode);
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data =  new {message  = "Disease Code updated successfully" }
                };
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
                logger.LogError(ex, "Error occurred while updating clinic category");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                var diseaseCode = await _context.DiseaseCodes.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);

                if (diseaseCode == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Disease code not found"
                        }
                    };
                    return response;
                }

                diseaseCode.IsActive = !diseaseCode.IsActive;
                _context.DiseaseCodes.Update(diseaseCode);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"Disease Code status updated to {(diseaseCode.IsActive ? "Active" : "Inactive")}." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating Disease Code status.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating Disease Code status." }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> DrugCtgIds)
        {
            try
            {
                var drugCategories = await _context.DrugCategories
                    .Where(b => DrugCtgIds.Contains(b.Id) && !b.IsDelete)
                    .ToListAsync();

                if (!drugCategories.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "No valid Disease Code found to delete." }
                    };
                }

                string? deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                foreach (var drugCtg in drugCategories)
                {
                    drugCtg.IsDelete = true;
                    drugCtg.UpdatedBy = deletedBy;
                }

                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Disease Code deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting multiple Disease Codes.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting Disease Codes." }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequest request)
        {
            try
            {
                IQueryable<DiseaseCodeModel> query = _context.DiseaseCodes.Where(b => !b.IsDelete);

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

                var diseaseCodeLists = await query.Select(data => new DiseaseCodeListDTO
                {
                    Id = data.Id,
                    DiseaseCode = data.DiseaseCode,
                    DiseaseName = data.DiseaseName,
                    Description = data.Description,
                    IsActive = data.IsActive,
                    CreatedAt = data.CreatedAt
                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = diseaseCodeLists
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while filtering Disease Code.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while filtering Disease Code." }
                };
            }
        }
    }
}

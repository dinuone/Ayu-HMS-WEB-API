using System.Net;
using System.Security.Claims;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DiseaseCodeRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode
{
    public class DiseaseCodeService : IDiseaseCodeService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IDiseaseCodeService> logger;
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
                var loggedInUser = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (loggedInUser == null) 
                {
                    response = new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Data = new
                        {
                            message = "User not logged in"
                        }
                    };
                    return response;
                }
                DiseaseCodeManagementModel DiseaseCode = new DiseaseCodeManagementModel
                {
                    DiseaseCode = request.DiseaseCode,
                    DiseaseName = request.DiseaseName,
                    Description = request.Description,
                    CreatedBy = loggedInUser,
                };
                _context.Set<DiseaseCodeManagementModel>().Add(DiseaseCode);
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Data = new {message = "Disease Code has been succefully created"}
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
                DiseaseCodeManagementModel? DiseaseCode = await _context.Set<DiseaseCodeManagementModel>().FindAsync(id);
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
                List<DiseaseCodeManagementModel> DiseaseCodes = await _context.DiseaseCodes.Where(dc => dc.IsDelete == false).ToListAsync();
                List<DiseaseCodeListDTO> DiseaseCodeListMapp = new List<DiseaseCodeListDTO>();

                foreach (var DiseaseCode in DiseaseCodes)
                {
                    DiseaseCodeListMapp.Add(new DiseaseCodeListDTO
                    {
                        DiseaseCodeId = DiseaseCode.DiseaseCodeId,
                        DiseaseCode = DiseaseCode.DiseaseCode,
                        DiseaseName = DiseaseCode.DiseaseName,
                        Description = DiseaseCode.Description,
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
                DiseaseCodeManagementModel? DiseaseCode = await _context.Set<DiseaseCodeManagementModel>().FindAsync(id);
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

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = DiseaseCode
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
                DiseaseCodeManagementModel? DiseaseCode = await _context.Set<DiseaseCodeManagementModel>().FindAsync(id);
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
                DiseaseCode.DiseaseCode = request.DiseaseCode;
                DiseaseCode.DiseaseName = request.DiseaseName;
                DiseaseCode.Description = request.Description;
                DiseaseCode.IsActive = request.IsActive;

                _context.Set<DiseaseCodeManagementModel>().Update(DiseaseCode);
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data =  new {message  = "Disease code has been updated"}
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
    }
}

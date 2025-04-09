using System.Net;
using System.Security.Claims;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.ClinicCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;


namespace Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory
{
    public class ClinicCategoryService: IClinicCategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<ClinicCategoryService> logger;
        TimeZoneInfo localTimeZone;

        public ClinicCategoryService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClinicCategoryService> logger)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> Create(AddClinicCategoryRequest request)
        {
            BaseResponse response;
            try
            {
                var loggedInUser = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (loggedInUser == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Data = new
                        {
                            message = "User not logged in"
                        }
                    };
                    return response;
                }

                ClinicCategoryModel clinicCategory = new ClinicCategoryModel
                {
                    Name = request.ClinicCategoryName,
                    Description = request.Description,
                    DayOfTheWeek = request.DayOfTheWeek,
                    CreatedBy = loggedInUser,
                };

                _context.Set<ClinicCategoryModel>().Add(clinicCategory);
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Data = new { message = "Clinic categoiry has been susccesffuly created"}
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
                logger.LogError(ex, "An error occurred while creating clinic category");
            }
            return response;
        }

        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                ClinicCategoryModel? clinicCategory = await _context.Set<ClinicCategoryModel>().FindAsync(id);
                if (clinicCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Clinic category not found"
                        }
                    };
                    return response;
                }

                clinicCategory.IsDelete = true;
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Clinic category deleted successfully"
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
                logger.LogError(ex, "Error occurred while deleting clinic category");
            }
            return response;
        }

        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;
            try
            {
                List<ClinicCategoryModel> clinicCategories = await _context.ClinicCategories.Where(clinic => clinic.IsDelete == false).ToListAsync();
                List<ClinicCategoryListDTO> ClinicCategoryListMapp = new List<ClinicCategoryListDTO>();
                foreach (var clinicCategory in clinicCategories)
                {
                    ClinicCategoryListMapp.Add(new ClinicCategoryListDTO
                    {
                        Id = clinicCategory.Id,
                        ClinicCategoryName = clinicCategory.Name,
                        Description = clinicCategory.Description,
                        DayOfTheWeek = clinicCategory.DayOfTheWeek,
                        CreatedBy = clinicCategory.CreatedBy,
                        CreatedAt = clinicCategory.CreatedAt,
                    });
                }
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = ClinicCategoryListMapp
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
                ClinicCategoryModel? clinicCategory = await _context.Set<ClinicCategoryModel>().FindAsync(id);
                if (clinicCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Clinic category not found"
                        }
                    };
                    return response;
                }
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = clinicCategory
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
                logger.LogError(ex, "Error occurred while getting Clinic category by id");
                
            }
            return response;
        }

        public async Task<BaseResponse> Update(int id, AddClinicCategoryRequest request)
        {
            BaseResponse response;
            try
            {
                ClinicCategoryModel? clinicCategory = await _context.Set<ClinicCategoryModel>().FindAsync(id);
                if (clinicCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Clinic category not found"
                        }
                    };
                    return response;
                }

                clinicCategory.Name = request.ClinicCategoryName;
                clinicCategory.Description = request.Description;
                clinicCategory.DayOfTheWeek = request.DayOfTheWeek;

                _context.Set<ClinicCategoryModel>().Update(clinicCategory);
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new {message ="Clinic category has been updated"}
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
                logger.LogError(ex, "Error occurred while updating clinic category");

            }
            return response;
        }
    }
}

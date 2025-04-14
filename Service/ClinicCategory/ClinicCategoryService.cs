using System.Net;
using System.Security.Claims;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
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


        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                ClinicCategoryModel? clinicCategory = await _context.ClinicCategories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);
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
                clinicCategory.IsActive = !clinicCategory.IsActive;
                _context.ClinicCategories.Update(clinicCategory);
                await _context.SaveChangesAsync();

              return new BaseResponse()
              {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new 
                    { 
                        message = $"Clinic category status updated to{(clinicCategory.IsActive ? "Active": "Inactive")}."
                    }
                };
               
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating clinic category status");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while updating clinic Category status."
                    }
                };
               
            }
           
        }

        public async Task<BaseResponse> DeleteAll(List<int> ClinicCtgIds)
        {
            try
            {
                var clinicCategories = await _context.ClinicCategories
                    .Where(c => ClinicCtgIds.Contains(c.Id) && !c.IsDelete).ToListAsync();

                if (!clinicCategories.Any()) 
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "No valid clinic categories found to delete." }
                    };
                }
                string ? deletedBy = httpContextAccessor.HttpContext.Items["CreatedBy"]?.ToString();
                foreach (var clinicCtg in clinicCategories)
                {
                    clinicCtg.IsDelete = true;
                    clinicCtg.UpdatedBy = deletedBy;
                }
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Clinic categories deleted successfully." }
                };
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error occurred while deleting clinic categories");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting clinic categories." }
                };
            } 
            
        }


        public async Task<BaseResponse> Filter(FilterRequest request) 
        {
            try
            {
                IQueryable<ClinicCategoryModel> query = _context.ClinicCategories.Where(c => !c.IsDelete);
                if (request.FromDate != null) 
                {
                    var fromDate = request.FromDate.Value.Date;
                    query = query.Where(c => c.CreatedAt.Date >= fromDate);
                }

                if (request.ToDate != null)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                    query = query.Where(c => c.CreatedAt.Date <= toDate);
                }


                if (request.IsActive != "All")
                {
                    if (request.IsActive == "active")
                        query = query.Where(sl => sl.IsActive);
                    else if (request.IsActive == "inactive")
                        query = query.Where(sl => !sl.IsActive);
                }

                var clinicCategories = await query.Select(data => new ClinicCategoryListDTO
                {
                    Id = data.Id,
                    ClinicCategoryName = data.Name,
                    Description = data.Description,
                    DayOfTheWeek = data.DayOfTheWeek,
                    CreatedBy = data.CreatedBy,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(data.CreatedAt, localTimeZone),
                    IsActive = data.IsActive
                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = clinicCategories
                };

            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error occurred while filtering clinic categories");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while filtering clinic categories." }
                };
            }
        }


    }
}

using System.Net;
using System.Security.Claims; // Add this line
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.TreatmentCategory
{
    public class TreatmentCategoryService : ITreatmentCategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<TreatmentCategoryService> logger;
        TimeZoneInfo localTimeZone;
        public TreatmentCategoryService(ApplicationDBContext context, ILogger<TreatmentCategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }
        
    

        public async Task<BaseResponse> Create(AddTreatmentCategoryRequest request)
        {
            BaseResponse response;

            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
               
                TreatmentCategoryModel treatmentCategory = new TreatmentCategoryModel
                {
                    Name = request.TreatmentName,
                    Description = request.Description,
                    CreatedBy = createdBy,
                };
                _context.Set<TreatmentCategoryModel>().Add(treatmentCategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Data = new { message = "Treatment category has been successfully created" }
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
                logger.LogError(ex, "Error occurred while creating treatment category");
                
            }
            return response;
        }

        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                TreatmentCategoryModel? treatmentCategory = await _context.Set<TreatmentCategoryModel>().FindAsync(id);
                if (treatmentCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment category not found"
                        }
                    };
                    return response;
                }
                treatmentCategory.IsDelete = true;
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Treatment category deleted successfully"
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
                logger.LogError(ex, "Error occurred while deleting treatment category");

            }
            return response;




        }

        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;
            try
            {
                List<TreatmentCategoryModel> treatmentCategories = await _context.TreatmentCategories.Where(t => t.IsDelete == false).ToListAsync();
                List<TreatmentCategoryListDTO> TreatmentCategoryListMapp = new List<TreatmentCategoryListDTO>();
                foreach (var treatmentCategory in treatmentCategories)
                {
                    TreatmentCategoryListMapp.Add(new TreatmentCategoryListDTO {
                        Id = treatmentCategory.Id,
                        TreatmentName = treatmentCategory.TreatmentName,
                        Description = treatmentCategory.Description,
                        CreatedBy = treatmentCategory.CreatedBy,
                        CreatedAt = treatmentCategory.CreatedAt,
                        
                    });
                }

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = TreatmentCategoryListMapp
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
                TreatmentCategoryModel? treatmentCategory = await _context.Set<TreatmentCategoryModel>().FindAsync(id);
                if (treatmentCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment category not found"
                        }
                    };
                    return response;
                }
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatmentCategory
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
                logger.LogError(ex, "Error occurred while getting treatment category by id");
            }
            return response;
        }

        public async Task<BaseResponse> Update(int id, AddTreatmentCategoryRequest request)
        {
            BaseResponse response;
            try
            {
                TreatmentCategoryModel? treatmentCategory = await _context.Set<TreatmentCategoryModel>().FindAsync(id);
                if (treatmentCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment category not found"
                        }
                    };
                    return response;
                    

                }

                string? updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                treatmentCategory.TreatmentName = request.TreatmentName;
                treatmentCategory.Description = request.Description;
                treatmentCategory.IsActive = request.IsActive;
                treatmentCategory.UpdatedBy = updatedBy;

                _context.Set<TreatmentCategoryModel>().Update(treatmentCategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Treatment category has been updated" }
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
                logger.LogError(ex, "Error occurred while updating treatment category");
                
            }
            return response;
        }
    }
    
}

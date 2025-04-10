using System.Net;
using System.Security.Claims; // Add this line
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
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
                    Data = new { message = "Treatment category created successfully" }
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
                        TreatmentName = treatmentCategory.Name,
                        Description = treatmentCategory.Description,
                        CreatedBy = treatmentCategory.CreatedBy,
                        IsActive = treatmentCategory.IsActive,
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

                TreatmentCategoryDataDTO treatmentCategoryData = new TreatmentCategoryDataDTO()
                {
                    Id = treatmentCategory.Id,
                    TreatmentName = treatmentCategory.Name,
                    Description = treatmentCategory.Description
                };

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatmentCategoryData
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

                treatmentCategory.Name = request.TreatmentName;
                treatmentCategory.Description = request.Description;
                treatmentCategory.UpdatedBy = updatedBy;

                _context.Set<TreatmentCategoryModel>().Update(treatmentCategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Treatment category updated successfully" }
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

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                var treatmentCategory = await _context.TreatmentCategories.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);

                if (treatmentCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment Category not found"
                        }
                    };
                    return response;
                }

                treatmentCategory.IsActive = !treatmentCategory.IsActive;
                _context.TreatmentCategories.Update(treatmentCategory);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"Treatment Category status updated to {(treatmentCategory.IsActive ? "Active" : "Inactive")}." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating Treatment Category status.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating Treatment Category status." }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> treatmentCtgIds)
        {
            try
            {
                var drugCategories = await _context.TreatmentCategories
                    .Where(b => treatmentCtgIds.Contains(b.Id) && !b.IsDelete)
                    .ToListAsync();

                if (!drugCategories.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "No valid Treatment Category found to delete." }
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
                    Data = new { message = "Treatment Categories deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting multiple Treatment Category.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting Treatment Category." }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequest request)
        {
            try
            {
                IQueryable<TreatmentCategoryModel> query = _context.TreatmentCategories.Where(b => !b.IsDelete);

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

                var treatmentCategoryList = await query.Select(data => new TreatmentCategoryListDTO
                {
                    Id = data.Id,
                    TreatmentName = data.Name,
                    Description = data.Description,
                    IsActive = data.IsActive,
                    CreatedAt = data.CreatedAt
                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatmentCategoryList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while filtering Treatment Category.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while filtering Treatment Category." }
                };
            }
        }
    }
    
}

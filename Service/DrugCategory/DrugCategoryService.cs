using System.Net;
using System.Security.Claims;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory
{
    public class DrugCategoryService : IDrugCategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IDrugCategoryService> logger;
        TimeZoneInfo localTimeZone;

        public DrugCategoryService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<DrugCategoryService> logger)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> Create(DrugCategoryRequest request)
        {
            BaseResponse response;
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
              
               DrugCategoryModel Drugcategory = new DrugCategoryModel
               {
                   Name = request.Name,
                   Description = request.Description,
                   CreatedBy = createdBy,
               };
                
                _context.Set<DrugCategoryModel>().Add(Drugcategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
               
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Drug Category created successfully." }
                };
                return response;
            }
            catch (Exception ex)
            {
                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while creating the Drug Category",
                        error = ex.Message
                    }
                };
                logger.LogError(ex, "An error occurred while creating new Drug Category");
                
            }
            return response;
        }

        public async Task<BaseResponse> Update(int id, DrugCategoryRequest request)
        {
            BaseResponse response;
            try
            {
                DrugCategoryModel? drugCategory = await _context.Set<DrugCategoryModel>().FindAsync(id);
                if (drugCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug Category not found"
                        }
                    };
                    return response;
                }
                string? updateBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                drugCategory.Name = request.Name;
                drugCategory.Description = request.Description;
                drugCategory.UpdatedBy = updateBy;

                _context.Set<DrugCategoryModel>().Update(drugCategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Drug Category updated successfully." }
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
                logger.LogError(ex, "An error occurred while updating Drug Category");
            }
            return response;
        }

        public async Task<BaseResponse> GetById(int id)
        {
            BaseResponse response;
            try
            {
                DrugCategoryModel? drugCategory = await _context.Set<DrugCategoryModel>().FindAsync(id);
                if (drugCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug Category not found"
                        }
                    };
                    return response;
                }
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugCategory
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
                logger.LogError(ex, "An error occurred while retrieving Drug Category by ID");
            }
            return response;
        }

        public async Task<BaseResponse> GetAll()
        {
            BaseResponse response;
            try
            {
                var drugCategories = await _context.drugcategories
                    .Where(d => d.IsDelete == false)
                    .Select(d => new DrugcategoryListDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        IsActive = d.IsActive,
                        CreatedAt = d.CreatedAt
                    })
                    .ToListAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugCategories
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
                logger.LogError(ex, "An error occurred while retrieving all Drug Categories");
            }

            return response;
        }


        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                DrugCategoryModel? drugCategory = await _context.Set<DrugCategoryModel>().FindAsync(id);
                if (drugCategory == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug Category not found"
                        }
                    };
                    return response;
                }

                drugCategory.IsDelete = true;
                await _context.SaveChangesAsync();

                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Branch deleted successfully" }
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
                logger.LogError(ex, "An error occurred while deleting Drug Category");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            try
            {
                var drugCategory = await _context.drugcategories.FirstOrDefaultAsync(u => u.Id == id && !u.IsDelete);

                if (drugCategory == null)
                {
                    return BaseResponse.CreateResponse("Branch not found.", HttpStatusCode.NotFound);
                }

                drugCategory.IsActive = !drugCategory.IsActive;
                _context.drugcategories.Update(drugCategory);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = $"Drug Category status updated to {(drugCategory.IsActive ? "Active" : "Inactive")}." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating drug category status.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating drug category status." }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> DrugCtgIds)
        {
            try
            {
                var drugCategories = await _context.drugcategories
                    .Where(b => DrugCtgIds.Contains(b.Id) && !b.IsDelete)
                    .ToListAsync();

                if (!drugCategories.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "No valid drug category found to delete." }
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
                    Data = new { message = "Drug Categories deleted successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting multiple drug category.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while deleting drug category." }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequest request)
        {
            try
            {
                IQueryable<DrugCategoryModel> query = _context.drugcategories.Where(b => !b.IsDelete);

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

                var drugcategoryList = await query.Select(data => new DrugcategoryListDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    IsActive = data.IsActive,
                    CreatedAt = data.CreatedAt
                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugcategoryList
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

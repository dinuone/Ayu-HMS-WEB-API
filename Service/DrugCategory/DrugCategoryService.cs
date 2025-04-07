using System.Net;
using System.Security.Claims;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;
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
               DrugCategoryModel Drugcategory = new DrugCategoryModel
               {
                   DrugCategory = request.DrugCategory,
                   Description = request.Description,
                   CreatedBy = loggedInUser,
               };
                _context.Set<DrugCategoryModel>().Add(Drugcategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Data = new { message = "Drug Category has been successfully created" }
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
                drugCategory.DrugCategory = request.DrugCategory;
                drugCategory.Description = request.Description;
                drugCategory.IsActive = request.IsActive;

                _context.Set<DrugCategoryModel>().Update(drugCategory);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Drug Category has been successfully updated" }
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
                var drugCategories = await _context.Set<DrugCategoryModel>().ToListAsync();
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
                    Data = new { message = "Drug Category has been successfully deleted" }
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
                logger.LogError(ex, "An error occurred while deleting Drug Category");
            }
            return response;
        }
    }
}

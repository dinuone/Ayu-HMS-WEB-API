using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Service.Treatment;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Drug
{
    public class DrugService : IDrugService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IDrugService> logger;
        TimeZoneInfo localTimeZone;


        public DrugService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IDrugService> logger)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> Create(AddDrugRequest request)
        {
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
               
                var drugExists = await _context.drugcategories
                    .AnyAsync(d => d.Id == request.DrugCategoryId
                    && d.IsActive == true && d.IsDelete == false);
                if (!drugExists)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest, 
                        Data = new
                        {
                            message = "Invalid drug category id"
                        }
                    };
                }

                DrugModel drug = new DrugModel
                {
                    Name = request.DrugName,
                    Brand = request.Brand,
                    Description = request.Description,
                    UnitPrice = request.UnitPrice,
                    DrugCategoryId = request.DrugCategoryId,
                    CreatedBy = createdBy,

                };

                await _context.drugs.AddAsync(drug);
                await _context.SaveChangesAsync();

                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Drug created successfully", drug.Id }
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error occured while creating a drug");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message,
                    }
                };
                
            }

        }

        public async Task<BaseResponse> Update(int id, AddDrugRequest request)
        {
            BaseResponse response;
            try
            {
                DrugModel? drug = await _context.Set<DrugModel>().FindAsync(id);
                if (drug == null) 
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug not found"
                        }
                    };
                    return response;
                }

                string? updatedBy = httpContextAccessor.HttpContext?.Items["UpdatedBy"]?.ToString();
                drug.Name = request.DrugName;
                drug.Brand = request.Brand;
                drug.Description = request.Description;
                drug.UnitPrice = request.UnitPrice;
                drug.DrugCategoryId = request.DrugCategoryId;
                drug.UpdatedBy = updatedBy;

                _context.Set<DrugModel>().Update(drug);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Drug updated successfully",
                        
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
                        message = ex.Message,
                    }
                };
                logger.LogError(ex, "An Error occured while updating a drug");

            }
            return response;
        }


        public async Task<BaseResponse> GetById(int id)
        {
            try
            {
                var drug = await _context.drugs
                    .Where(d => d.Id == id && d.IsDelete == false).Join(
                    _context.drugcategories,
                    drug => drug.DrugCategoryId,
                    drugCategory => drugCategory.Id,
                    (drug, drugCategory) => new
                    {
                        drug.Id,
                        drug.Name,
                        drug.Brand,
                        drug.Description,
                        drug.UnitPrice,
                        drug.CreatedBy,
                        drug.CreatedAt,
                        drug.IsActive,
                        DrugCategoryName = drugCategory.Name,
                        DrugCategoryId = drugCategory.Id


                    }
                    ).FirstOrDefaultAsync();

                if (drug == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug not found"
                        }
                    };
                }

                var drugDto = new DrugDataDTO
                {
                    Id = drug.Id,
                    DrugName = drug.Name,
                    Brand = drug.Brand,
                    Description = drug.Description,
                    UnitPrice = drug.UnitPrice,
                    DrugCategory = new DrugCategoryDataDTO
                    {
                        Id = drug.DrugCategoryId, 
                        DrugCategoryName = drug.DrugCategoryName
                    }

                };

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugDto
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error occurred while fetching the drug by ID");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };
            }
        }
        public async Task<BaseResponse> GetAll()
        {
            try
            {
                var drugs = await _context.drugs.Where(d => d.IsDelete == false).Join(
                    _context.drugcategories, 
                    drug => drug.DrugCategoryId,
                    drugCategory => drugCategory.Id,
                    (drug, drugcategory) => new 
                    {
                        drug.Id,
                        drug.Name,
                        drug.Brand,
                        drug.Description,
                        drug.UnitPrice,
                        drug.CreatedBy,
                        drug.CreatedAt,
                        drug.IsActive,
                        DrugCategoryName = drugcategory.Name,
                    }
                    ).ToListAsync();

                var response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugs
                };
                return response;
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while retrieving all drugs");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }


        public async Task<BaseResponse> Delete(int id)
        {
            BaseResponse response;
            try
            {
                DrugModel? drug = await _context.Set<DrugModel>().FindAsync(id);
                if (drug == null) 
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug not found"
                        }
                    };
                    return response;

                }
                drug.IsDelete = true;
                _context.Set<DrugModel>().Update(drug);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Drug deleted successfully"
                    }
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
                logger.LogError(ex, "An error occurred while deleting the drug");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                var drug = await _context.drugs.FirstOrDefaultAsync(d => d.Id == id
                && !d.IsDelete);

                if (drug == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Drug not found"
                        }
                    };
                    return response;
                }


                drug.IsActive = !drug.IsActive;
                _context.drugs.Update(drug);
                await _context.SaveChangesAsync();


                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = $"Treatment status updated to {(drug.IsActive ? "Active" : "Inactive")}."
                    }
                };
            }

           


            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while updating the drug status");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while updating drug status."
                    }
                };
            }
        }

        public async Task<BaseResponse> DeleteAll(List<int> drugIds)
        {
            try
            {
                var drugs = await _context.Treatments
                    .Where(d => drugIds.Contains(d.Id) && !d.IsDelete).ToListAsync();

                if (!drugs.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "No drugs found to delete."
                        }
                    };
                }
                string deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                foreach (var drug in drugs)
                {
                    drug.IsDelete = true;
                    drug.UpdatedBy = deletedBy;
                }

                await _context.SaveChangesAsync();

                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Selected drugs deleted successfully."
                    }
                };
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while deleting drugs");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while deleting drugs."
                    }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequest request)
        {
            try
            {
                IQueryable<DrugModel> query = _context.drugs.Where(d => !d.IsDelete);
                if (request.FromDate != null)
                {
                    var fromDate = request.FromDate.Value.Date;
                    query = query.Where(d => d.CreatedAt.Date >= fromDate);
                }
                if (request.ToDate != null)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                    query = query.Where(d => d.CreatedAt.Date <= toDate);
                }
                if (request.IsActive != "All")
                {
                    if (request.IsActive == "active")
                        query = query.Where(d => d.IsActive);
                    else if (request.IsActive == "inactive")
                        query = query.Where(d => !d.IsActive);
                }

                var drugList = await query.Select(data => new DrugListDTO
                {
                    Id = data.Id,
                    DrugName = data.Name, // Set the required 'DrugName' property
                    Brand = data.Brand,
                    Description = data.Description,
                    UnitPrice = data.UnitPrice,
                    //DrugCategory = new DrugCategoryDataDTO
                    //{
                    //    Id = (int)data.DrugCategoryId, // Explicitly cast 'long' to 'int'
                    //    DrugCategoryName = _context.drugcategories
                    //        .Where(dc => dc.Id == data.DrugCategoryId)
                    //        .Select(dc => dc.Name)
                    //        .FirstOrDefault() ?? string.Empty // Handle possible null reference
                    //},
                    CreatedBy = data.CreatedBy,
                    IsActive = data.IsActive,
                    CreatedAt = data.CreatedAt
                }).ToListAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while filtering drugs");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }

      

       

       
    }
}

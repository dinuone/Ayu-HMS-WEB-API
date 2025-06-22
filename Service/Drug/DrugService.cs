using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DrugDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
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

                

                var drugExists = await _context.DrugCategories
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
                    DrugCode = request.DrugCode,
                    Brand = request.Brand,
                    Description = request.Description,
                    UnitPrice = request.UnitPrice,
                    DrugCategoryId = request.DrugCategoryId,
                    CreatedBy = createdBy,

                };

                await _context.Drugs.AddAsync(drug);
                await _context.SaveChangesAsync();

                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Drug created successfully" }
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
                drug.DrugCode = request.DrugCode;
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
                var drug = await _context.Drugs
                    .Where(d => d.Id == id && d.IsDelete == false).Join(
                    _context.DrugCategories,
                    drug => drug.DrugCategoryId,
                    drugCategory => drugCategory.Id,
                    (drug, drugCategory) => new
                    {
                        drug.Id,
                        drug.Name,
                        drug.DrugCode,
                        drug.Brand,
                        drug.Description,
                        drug.UnitPrice,
                        drug.CreatedBy,
                        drug.CreatedAt,
                        drug.IsActive,
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
                    DrugCode = drug.DrugCode,
                    Brand = drug.Brand,
                    Description = drug.Description,
                    UnitPrice = drug.UnitPrice,
                    DrugCategory = drug.DrugCategoryId
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
                var drugs = await _context.Drugs
                    .Where(d => d.IsDelete == false)
                    .Join(
                        _context.DrugCategories,
                        drug => drug.DrugCategoryId,
                        drugCategory => drugCategory.Id,
                        (drug, drugCategory) => new DrugListDTO
                        {
                            Id = drug.Id,
                            DrugName = drug.Name,
                            DrugCode = drug.DrugCode,
                            Brand = drug.Brand,
                            Description = drug.Description,
                            UnitPrice = drug.UnitPrice,
                            CreatedBy = drug.CreatedBy,
                            CreatedAt = drug.CreatedAt,
                            IsActive = drug.IsActive,
                            DrugCategory = new DrugCategoryDataDTO
                            {
                                Id = drugCategory.Id,
                                DrugCategoryName = drugCategory.Name
                            }
                        })
                    .ToListAsync();

                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugs
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving all drugs");
                return new BaseResponse
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
                var drug = await _context.Drugs.FirstOrDefaultAsync(d => d.Id == id
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
                _context.Drugs.Update(drug);
                await _context.SaveChangesAsync();


                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = $"Drug status updated to {(drug.IsActive ? "Active" : "Inactive")}."
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
                var drugs = await _context.Drugs
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

        public async Task<BaseResponse> Filter(FilterRequestDrug request)
        {
            try
            {
                IQueryable<DrugModel> query = _context.Drugs.Where(t => !t.IsDelete);

                if (request.DrugCategoryIds != null && request.DrugCategoryIds.Any())
                {
                    query = query.Join(
                        _context.DrugCategories,
                        drug => drug.DrugCategoryId,
                        category => category.Id,
                        (drug, category) => new { drug, category }
                    )
                    .Where(dc => request.DrugCategoryIds.Contains(dc.category.Id))
                    .Select(dc => dc.drug);
                }

                if (request.FromDate != null)
                {
                    var fromDate = request.FromDate.Value.Date;
                    query = query.Where(t => t.CreatedAt.Date >= fromDate);
                }

                if (request.ToDate != null)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                    query = query.Where(t => t.CreatedAt.Date <= toDate);
                }

                if (!string.IsNullOrEmpty(request.IsActive) && request.IsActive != "All")
                {
                    if (request.IsActive == "active")
                        query = query.Where(t => t.IsActive);
                    else if (request.IsActive == "inactive")
                        query = query.Where(t => !t.IsActive);
                }

                var treatmentData = await query.ToListAsync();

                // Now build the DTOs
                var drugcategoryLists = new List<DrugListDTO>();
                foreach (var data in treatmentData)
                {
                    DrugCategoryDataDTO categoryName = await GetDrugCategoryName(data.DrugCategoryId, _context);
                    drugcategoryLists.Add(new DrugListDTO
                    {
                        Id = data.Id,
                        Description = data.Description,
                        Brand = data.Brand,
                        DrugCategory = categoryName,
                        CreatedAt = data.CreatedAt,
                        CreatedBy = data.CreatedBy,
                        DrugName = data.Name,
                        IsActive = data.IsActive,
                        UnitPrice = data.UnitPrice

                    });
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = drugcategoryLists
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while filtering drugs");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while filtering drugs." }
                };
            }
        }


        public static async Task<DrugCategoryDataDTO> GetDrugCategoryName(long id, ApplicationDBContext context)
        {
            var treatmentCategory = await context.DrugCategories
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (treatmentCategory != null)
            {
                return new DrugCategoryDataDTO
                {
                    Id = treatmentCategory.Id,
                    DrugCategoryName = treatmentCategory.Name
                };
            }

            return new DrugCategoryDataDTO();
        }


    

public async Task<BaseResponse> ImportDrugsFromCsv(DrugImportFromExcelRequest request)
        {
            try
            {
                if (request.Drugs == null || !request.Drugs.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "No drugs provided for import." }
                    };
                }

                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                foreach (var drugRequest in request.Drugs)
                {
                    var drugExists = await _context.DrugCategories
                        .AnyAsync(d => d.Id == drugRequest.DrugCategoryId
                        && d.IsActive == true && d.IsDelete == false);

                    if (!drugExists)
                    {
                        return new BaseResponse
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Data = new { message = $"Invalid drug category id: {drugRequest.DrugCategoryId}" }
                        };
                    }

                    DrugModel drug = new DrugModel
                    {
                        Name = drugRequest.DrugName,
                        DrugCode = drugRequest.DrugCode,
                        Brand = drugRequest.Brand,
                        Description = drugRequest.Description,
                        UnitPrice = drugRequest.UnitPrice,
                        DrugCategoryId = drugRequest.DrugCategoryId,
                        CreatedBy = createdBy
                    };

                    await _context.Drugs.AddAsync(drug);
                }

                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Drugs imported successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while importing drugs from CSV");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }
    }
}

     

using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.DiseaseCodeDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentCategoryDTO;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.TreatmentDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Treatment
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<ITreatmentService> logger;
        TimeZoneInfo localTimeZone;
        public TreatmentService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<ITreatmentService> logger)
        {
            _context = context;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> Create(TreatmentCreateRequest request)
        {
            try
            {
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                var categoryExists = await _context.TreatmentCategories
                    .AnyAsync(c => c.Id == request.TreatmentCategoryId 
                    && c.IsActive == true && c.IsDelete == false);
                if (!categoryExists)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Invalid Treatment Category Id" }
                    };
                }

               
                TreatmentModel treatment = new TreatmentModel
                {
                    Name = request.TreatmentName,
                    TreatmentPrice = request.TreatmentPrice,
                    TreatmentCategoryId = request.TreatmentCategoryId,
                    CreatedBy = createdBy,
                };  

              
                await _context.Treatments.AddAsync(treatment);
                await _context.SaveChangesAsync();

                
                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Treatment created successfully", treatment.Id }
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating a treatment");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }
        public async Task<BaseResponse> Update(int id, TreatmentCreateRequest request)
        {
            BaseResponse response;
            try
            {
                TreatmentModel? treatment = await _context.Set<TreatmentModel>().FindAsync(id);
                if (treatment == null) 
                {
                  response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment not found"
                        }
                    };
                  
                    return response;

                }
                string? updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                treatment.Name = request.TreatmentName;
                treatment.TreatmentPrice = request.TreatmentPrice;
                treatment.TreatmentCategoryId = request.TreatmentCategoryId;
                treatment.UpdatedBy = updatedBy;

                _context.Set<TreatmentModel>().Update(treatment);
                await _context.SaveChangesAsync();
                 response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Treatment updated successfully"
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
                logger.LogError(ex, "An error occurred while updating the treatment");

            }
            return response;
        }
        public async Task<BaseResponse> GetById(int id)
        {
            try
            {
               
                TreatmentModel? treatment = await _context.Treatments.Where(t => t.Id == id && t.IsDelete == false).FirstOrDefaultAsync();

                if (treatment == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Treatment not found" }
                    };
                }

                var treatmentDto = new TreatmentDataDTO
                {
                    Id = treatment.Id,
                    TreatmentName = treatment.Name,
                    TreatmentPrice = treatment.TreatmentPrice,
                    TreatmentCategory = treatment.TreatmentCategoryId,
                };
              


                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatmentDto
                };

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving the treatment by ID");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = ex.Message }
                };
            }
        }
        public async Task<BaseResponse> GetAll(bool IsActive)
        {
           
            try
            {
                var treatments = await _context.Treatments
                   .Where(t => !t.IsDelete && t.IsActive == IsActive)
                   .Join(
                       _context.TreatmentCategories,
                       treatment => treatment.TreatmentCategoryId,
                       category => category.Id,
                       (treatment, category) => new TreatmentListDTO
                       {
                           Id = treatment.Id,
                           TreatmentName = treatment.Name,
                           TreatmentPrice = treatment.TreatmentPrice,
                           IsActive = treatment.IsActive,
                           CreatedBy = treatment.CreatedBy,
                           CreatedAt = treatment.CreatedAt,
                           TreatmentCategory = new TreatmentCategoryDataDTO
                           {
                               Name = category.Name
                           }
                       }
                   ).ToListAsync();


                var response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatments
                };
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving all treatments");
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
                TreatmentModel? treatment = await _context.Set<TreatmentModel>().FindAsync(id);
                if (treatment == null)
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment not found"
                        }
                    };
                    return response;
                }
                treatment.IsDelete = true;
                _context.Set<TreatmentModel>().Update(treatment);
                await _context.SaveChangesAsync();
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Treatment deleted successfully"
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
                logger.LogError(ex, "An error occurred while deleting the treatment");
            }
            return response;
        }

        public async Task<BaseResponse> UpdateStatus(int id)
        {
            BaseResponse response;
            try
            {
                var treatment = await _context.Treatments.FirstOrDefaultAsync(u => u.Id == id 
                && !u.IsDelete);
                if (treatment == null) 
                {
                    response = new BaseResponse()
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new
                        {
                            message = "Treatment not found"
                        }
                    };
                    return response;
                }
                treatment.IsActive = !treatment.IsActive;
                _context.Treatments.Update(treatment);
                await _context.SaveChangesAsync();

                return new BaseResponse 
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new 
                    { 
                        message = $"Treatment status updated to {(treatment.IsActive ? "Active" : "Inactive")}."
                    }
                };
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "An error occurred while updating the treatment status");
                return new BaseResponse()
                {
                    StatusCode= (int)HttpStatusCode.InternalServerError,
                    Data = new 
                    {
                        message = "An error occurred while updating Treatment  status."
                    }
                };
               
            }
            
        }

        public async Task<BaseResponse> DeleteAll(List<int> treatmentIds)
        {
            try
            {
                var treatments = await _context.Treatments
                    .Where(t => treatmentIds.Contains(t.Id) && !t.IsDelete).ToListAsync();

                if (!treatments.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new
                        {
                            message = "No treatments found to delete."
                        }
                    };
                }
                string deletedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                foreach (var treatment in treatments)
                {
                    treatment.IsDelete = true;
                    treatment.UpdatedBy = deletedBy;
                }

                await _context.SaveChangesAsync();

                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new
                    {
                        message = "Selected treatments deleted successfully."
                    }
                };
            }
            catch (Exception ex)
            {

               logger.LogError(ex, "An error occurred while deleting treatments");
                return new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = "An error occurred while deleting treatments."
                    }
                };
            }
        }

        public async Task<BaseResponse> Filter(FilterRequestTreatment request)
        {
            try
            {
                IQueryable<TreatmentModel> query = _context.Treatments.Where(t => !t.IsDelete);

                if (request.TreatmentCategoryIds != null && request.TreatmentCategoryIds.Any())
                {
                    query = query.Join(
                        _context.TreatmentCategories,
                        treatment => treatment.TreatmentCategoryId,
                        category => category.Id,
                        (treatment, category) => new { treatment, category }
                    )
                    .Where(tc => request.TreatmentCategoryIds.Contains(tc.category.Id))
                    .Select(tc => tc.treatment);
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
                var treatmentList = new List<TreatmentListDTO>();
                foreach (var data in treatmentData)
                {
                    TreatmentCategoryDataDTO categoryName = await GetTreatmentCategory(data.TreatmentCategoryId, _context);
                    treatmentList.Add(new TreatmentListDTO
                    {
                        Id = data.Id,
                        TreatmentName = data.Name,
                        TreatmentPrice = data.TreatmentPrice,
                        TreatmentCategory = categoryName,
                        CreatedAt = data.CreatedAt,
                        IsActive = data.IsActive,
                    });
                }

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatmentList
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while filtering treatments");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while filtering Treatment." }
                };
            }
        }


        public static async Task<TreatmentCategoryDataDTO> GetTreatmentCategory(long id, ApplicationDBContext context)
        {
            var treatmentCategory = await context.TreatmentCategories
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (treatmentCategory != null)
            {
                return new TreatmentCategoryDataDTO
                {
                    Id = treatmentCategory.Id,
                    Name = treatmentCategory.Name
                };
            }

            return new TreatmentCategoryDataDTO(); 
        }


    }

}

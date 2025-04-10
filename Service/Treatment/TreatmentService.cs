using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentRequest;
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

                var categoryExists = await _context.TreatmentCategories.AnyAsync(c => c.Id == request.TreatmentCategoryId);
                if (!categoryExists)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Invalid TreatmentCategoryId" }
                    };
                }

               
                TreatmentModel treatment = new TreatmentModel
                {
                    Name = request.TreatmentName,
                    TreatmentPrice = request.TreatmentPrice,
                    TreatmentCategoryId = request.TreatmentCategoryId,
                    CreatedBy = createdBy,
                    IsActive = true,
                };

              
                await _context.Treatments.AddAsync(treatment);
                await _context.SaveChangesAsync();

                
                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.Created,
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
            try
            {
                TreatmentModel? treatment = await _context.Set<TreatmentModel>().FindAsync(id);
                if (treatmentModel == null) 
                {
                    var response = new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Treatment not found" }
                    };
                    return response;

                }
                string? updatedBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();
                treatment.Name = request.TreatmentName;
                treatment.TreatmentPrice = request.TreatmentPrice;
                treatment.TreatmentCategoryId = request.TreatmentCategoryId;
                treatment.UpdatedBy = updatedBy;
                treatment.UpdatedAt = DateTime.UtcNow;
                treatment.IsActive = request.IsActive;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<BaseResponse> GetById(int id)
        {
            try
            {
               
                var treatment = await _context.Treatments
                    .Where(t => t.Id == id && t.IsDelete == false)
                    .Join(
                        _context.TreatmentCategories,
                        treatment => treatment.TreatmentCategoryId,
                        category => category.Id,
                        (treatment, category) => new
                        {
                            treatment.Id,
                            treatment.Name,
                            treatment.TreatmentPrice,
                            treatment.CreatedBy,
                            treatment.CreatedAt,
                            treatment.IsActive,
                            CategoryName = category.Name,
                        }
                    )
                    .FirstOrDefaultAsync();

                if (treatment == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Treatment not found" }
                    };
                }

                
                var response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = treatment
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
        public async Task<BaseResponse> GetAll()
        {
           
            try
            {
                var treatments = await _context.Treatments.Where(t => t.IsDelete == false).Join(
                    _context.TreatmentCategories,
                    treatment => treatment.TreatmentCategoryId,
                    category => category.Id,
                    (treatment, category) => new
                    {
                        treatment.Id,
                        treatment.Name,
                        treatment.TreatmentPrice,
                        treatment.CreatedBy,
                        treatment.CreatedAt, 
                        treatment.IsActive,
                        CategoryName = category.Name,
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
        public Task<BaseResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}

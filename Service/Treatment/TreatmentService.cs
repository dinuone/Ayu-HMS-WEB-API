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
        public Task<BaseResponse> Create(TreatmentCreateRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<BaseResponse> Update(int id, TreatmentCreateRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<BaseResponse> GetById(int id)
        {
            throw new NotImplementedException();
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

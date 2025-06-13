using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatesRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.RatesDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Rates
{
    public class RatesService : IRatesService
    {

        private readonly ApplicationDBContext context;
        private readonly ILogger<RatesService> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
             
        public RatesService(ApplicationDBContext context, ILogger<RatesService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;

        }

        public async Task<BaseResponse> GetRates()
        {
            try
            {
                var rate = await context.Rates.FirstOrDefaultAsync();

                if (rate == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = new { message = "Rates record not found." }
                    };
                }

               

                RatesDataDTO ratesDataDTO = new RatesDataDTO()
                {
                    HospitalCharge = rate.HospitalCharge,
                    CustomCharge = rate.CustomCharges,
                };

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = ratesDataDTO
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving rates.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while retrieving rates." }
                };
            }
        }


        public async Task<BaseResponse> UpdateRates(RatesUpdateRequest request)
        {
            try
            {
                var rate = await context.Rates.FirstOrDefaultAsync();

                if (rate == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Data = new { message = "Rates record not found." }
                    };
                }
                string? createdBy = httpContextAccessor.HttpContext?.Items["CreatedBy"]?.ToString();

                rate.HospitalCharge = request.HospitalCharge;
                rate.CustomCharges = request.CustomCharge;
                rate.UpdatedBy = createdBy;
                rate.UpdatedAt = DateTime.UtcNow;

                context.Rates.Update(rate);
                await context.SaveChangesAsync();

                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = new { message = "Rates updated successfully." }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating rates.");
                return new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new { message = "An error occurred while updating rates." }
                };
            }
        }
    
    }
}

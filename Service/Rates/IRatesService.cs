using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatesRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Rates
{
    public interface IRatesService
    {
        Task<BaseResponse> UpdateRates(RatesUpdateRequest request);
        Task<BaseResponse> GetRates();
    }
}

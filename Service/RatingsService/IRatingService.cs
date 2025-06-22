using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatingsRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.RatingsService
{
    public interface IRatingService
    {
        Task<BaseResponse> AddRating(AddRatingRequest request);
        Task<BaseResponse> GetTotalRatingCount();

        Task<BaseResponse> GetrateCountByOfRating();
    }
}

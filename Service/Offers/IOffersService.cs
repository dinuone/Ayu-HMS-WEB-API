using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.OffersRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Offers
{
    public interface IOffersService
    {
        Task<BaseResponse> Create(AddOffersRequest request);
        Task<BaseResponse> Update(int id, AddOffersRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll(bool IsActive);
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> DeleteAll(List<int> OfferIds);
        Task<BaseResponse> Filter(FilterRequest request);
    }
}

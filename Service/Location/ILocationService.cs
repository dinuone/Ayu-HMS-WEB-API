using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Location
{
    public interface ILocationService
    {
        Task<BaseResponse> GetProvinces();
        Task<BaseResponse> GetDistricts(int provinceId);
        Task<BaseResponse> GetCities(int districtId);
        Task CheckCityExist(string districtName, string name);
    }
}

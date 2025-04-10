using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory
{
    public interface IClinicCategoryService
    {
        Task<BaseResponse>Create(AddClinicCategoryRequest request);
        Task<BaseResponse> Update(int id, AddClinicCategoryRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> DeleteAll(List<int> ClinicCtgIds);
        Task<BaseResponse> Filter(FilterRequest request);
    }
}

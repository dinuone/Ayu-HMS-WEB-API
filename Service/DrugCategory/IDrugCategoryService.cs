using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory
{
    public interface IDrugCategoryService
    {
        Task<BaseResponse> Create(DrugCategoryRequest request);
        Task<BaseResponse> Update(int id, DrugCategoryRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> DeleteAll(List<int> DrugCtgIds);
        Task<BaseResponse> Filter(FilterRequest request);
    }
}

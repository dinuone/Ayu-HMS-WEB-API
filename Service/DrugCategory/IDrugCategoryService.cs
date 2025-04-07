using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;
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
    }
}

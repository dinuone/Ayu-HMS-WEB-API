
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentCategory;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.TreatmentCategory
{
    public interface ITreatmentCategoryService
    {
        Task<BaseResponse> Create(AddTreatmentCategoryRequest request);
        Task<BaseResponse> Update(int id, AddTreatmentCategoryRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(int id);
    }
}

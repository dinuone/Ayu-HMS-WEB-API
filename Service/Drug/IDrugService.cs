using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Drug
{
    public interface IDrugService
    {
        Task<BaseResponse> Create(AddDrugRequest request);
        Task<BaseResponse> Update(int id, AddDrugRequest request);
        Task<BaseResponse> GetById(int id);
        Task<BaseResponse> GetAll();
        Task<BaseResponse> Delete(int id);
        Task<BaseResponse> UpdateStatus(int id);
        Task<BaseResponse> DeleteAll(List<int> treatmentIds);
        Task<BaseResponse> Filter(FilterRequest request);
    }
}

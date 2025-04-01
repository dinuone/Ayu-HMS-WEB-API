using BookInventory.DTOs.Request.Product;
using BookInventory.Helpers;

namespace BookInventory.Service.Product
{
    public interface IProductService
    {
        Task<BaseResponse> Create(CreateProductRequest request);
        Task<BaseResponse> Update();
        Task<BaseResponse> Delete();
        Task<BaseResponse> List();
        Task<BaseResponse> Get(long id);
    }
}

using BookInventory.DTOs.Request.Product;
using BookInventory.Helpers;
using BookInventory.Models;
using BookInventory.Service.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using static BookInventory.Helpers.GlobalAttributeHelper;

namespace BookInventory.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext dBContext;
        private readonly ILogger<ProductService> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        TimeZoneInfo localTimeZone;

        public ProductService(ApplicationDBContext dBContext, ILogger<ProductService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.dBContext = dBContext;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> Create(CreateProductRequest request)
        {
            BaseResponse response;
            try
            {
                var loggedInUser = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                ProductModel product = new ProductModel();
                product.BarCode = request.BarCode;
                product.Name = request.Name;
                product.Price = request.Price;
                product.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);
                product.CreatedBy = loggedInUser;

                await dBContext.Products.AddAsync(product);
                await dBContext.SaveChangesAsync();

                response = new BaseResponse
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    Data = new { message = "Product created successfully." }
                };

                return response;

            }
            catch(Exception ex) 
            {
                response = new BaseResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Data = new
                    {
                        message = ex.Message
                    }
                };

                logger.LogInformation("Product Create Exception :::::" + ex);

                return response;
            }

        }

        public Task<BaseResponse> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Get(long id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> List()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> Update()
        {
            throw new NotImplementedException();
        }
    }
}

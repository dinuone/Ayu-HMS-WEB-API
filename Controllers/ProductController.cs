using BookInventory.DTOs.Request.Product;
using BookInventory.Helpers;
using BookInventory.Service.Auth;
using BookInventory.Service.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;   
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            BaseResponse response = await productService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

    }
}

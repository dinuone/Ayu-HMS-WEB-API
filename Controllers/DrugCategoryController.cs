using Microsoft.AspNetCore.Mvc;
using Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/drug-category")]
    [ApiController]
    public class DrugCategoryController : Controller
    {

        private readonly IDrugCategoryService drugCategoryService;
        public DrugCategoryController(IDrugCategoryService drugCategoryService)
        {
            this.drugCategoryService = drugCategoryService;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DrugCategoryRequest request)
        {
            var response = await drugCategoryService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, DrugCategoryRequest request)
        {
            var response = await drugCategoryService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await drugCategoryService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        [HttpGet("list/{isActive}")]
        public async Task<IActionResult> GetAll(bool isActive = true)
        {
            var response = await drugCategoryService.GetAll(isActive);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await drugCategoryService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) 
            BaseResponse response = await drugCategoryService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await drugCategoryService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterRequest request)
        {
            BaseResponse response = await drugCategoryService.Filter(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }


    }
}

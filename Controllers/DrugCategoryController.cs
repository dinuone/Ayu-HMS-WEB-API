using Microsoft.AspNetCore.Mvc;
using Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;

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
        public async Task<IActionResult> GetAll()
        {
            var response = await drugCategoryService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await drugCategoryService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }


    }
}

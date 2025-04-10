using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentCategory;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode;
using Harendra_Ayurveda_HMS_WEB_API.Service.TreatmentCategory;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/treatment-category")]
    [ApiController]
    public class TreatmentCategoryController : Controller
    {
      
        private readonly ITreatmentCategoryService _treatmentCategoryService;
        public TreatmentCategoryController(ITreatmentCategoryService treatmentCategoryService)
        {
            _treatmentCategoryService = treatmentCategoryService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AddTreatmentCategoryRequest request) 
        {
            var response = await _treatmentCategoryService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPut("update/{id}")]

        public async Task<IActionResult> Update(int id, AddTreatmentCategoryRequest request)
        {
            var response = await _treatmentCategoryService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }


        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _treatmentCategoryService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _treatmentCategoryService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _treatmentCategoryService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) 
            BaseResponse response = await _treatmentCategoryService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await _treatmentCategoryService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterRequest request)
        {
            BaseResponse response = await _treatmentCategoryService.Filter(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

    }
}

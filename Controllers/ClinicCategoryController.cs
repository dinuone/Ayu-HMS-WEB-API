using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/clinic-category")]
    [ApiController]
    public class ClinicCategoryController : Controller
    {
       private readonly IClinicCategoryService clinicCategoryService;
        public ClinicCategoryController(IClinicCategoryService clinicCategoryService)
        {
            this.clinicCategoryService = clinicCategoryService;
        }

        [Authorize]
        [HttpPost("Create")]

        public async Task<IActionResult> Create(AddClinicCategoryRequest request)
        {
            var response = await clinicCategoryService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, AddClinicCategoryRequest request)
        {
            var response = await clinicCategoryService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await clinicCategoryService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await clinicCategoryService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await clinicCategoryService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }


        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) 
            BaseResponse response = await clinicCategoryService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await clinicCategoryService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterRequestClinic request)
        {
            BaseResponse response = await clinicCategoryService.Filter(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
    }
}

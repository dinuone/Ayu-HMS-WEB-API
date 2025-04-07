using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/cliniccategory")]
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


    }
}

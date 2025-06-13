using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DiseaseCodeRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode;
using Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/disease-code")]
    [ApiController]
    public class DiseaseCodeController : Controller
    {
        private readonly IDiseaseCodeService diseaseCodeService;
        public DiseaseCodeController(IDiseaseCodeService diseaseCodeService)
        {
            this.diseaseCodeService = diseaseCodeService;
        }
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DiseaseCodeRequest request)
        {
            var response = await diseaseCodeService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, DiseaseCodeRequest request)
        {
            var response = await diseaseCodeService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await diseaseCodeService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await diseaseCodeService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await diseaseCodeService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) 
            BaseResponse response = await diseaseCodeService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await diseaseCodeService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterRequest request)
        {
            BaseResponse response = await diseaseCodeService.Filter(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

    }
}

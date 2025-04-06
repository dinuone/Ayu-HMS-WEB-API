using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DiseaseCodeRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/diseasecode")]
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

    }
}

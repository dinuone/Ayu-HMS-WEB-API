using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.TreatmentRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Treatment;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/treatment")]
    [ApiController]
    public class TreatmentController : Controller
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(TreatmentCreateRequest request)
        {
            var response = await _treatmentService.Create(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, TreatmentCreateRequest request)
        {
            var response = await _treatmentService.Update(id, request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _treatmentService.GetById(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpGet("list")]
        [HttpGet("list/{isActive}")]
        public async Task<IActionResult> GetAll(bool isActive = true)
        {
            var response = await _treatmentService.GetAll(isActive);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _treatmentService.Delete(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }


        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            var response = await _treatmentService.DeleteAll(ids);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var response = await _treatmentService.UpdateStatus(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterRequestTreatment request)
        {
            var response = await _treatmentService.Filter(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}

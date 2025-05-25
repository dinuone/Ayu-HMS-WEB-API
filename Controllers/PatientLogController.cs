using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientLog;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.PatientLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/patient-log")]
    [ApiController]
    public class PatientLogController : Controller
    {
        private readonly IPatientLogService _patientLogService;
        public PatientLogController(IPatientLogService patientLogService)
        {
            this._patientLogService = patientLogService;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PatientLogCreateRequest request)
        {
            var response = await _patientLogService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _patientLogService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _patientLogService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(long id, PatientLogCreateRequest request)
        {
            var response = await _patientLogService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };


        }
    }
}

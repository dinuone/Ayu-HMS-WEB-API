using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.PatientVisit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/patient-visit")]
    [ApiController]
    public class PatientVisitController : ControllerBase
    {
        private readonly IPatientVisitService patientVisitService;
        public PatientVisitController(IPatientVisitService patientVisitService)
        {
            this.patientVisitService = patientVisitService; 
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AddPatientVisitRequest request)
        {
            var response = await patientVisitService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
    }
}

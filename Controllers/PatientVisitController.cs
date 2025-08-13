using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.PatientVisit;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Response.PatientVisitDTO;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.Patient;
using Harendra_Ayurveda_HMS_WEB_API.Service.PatientVisit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

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

        [Authorize]
        [HttpGet("get-available-clinics-or-treatments/{vistiType}")]
        public async Task<IActionResult> GetAvailableClinicOrTreatments(PatientVisitTypes vistiType)
        {
            var response = await patientVisitService.GetAvailableClinicsOrTreatments(vistiType);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get-available-doctors-for-clinic/{clinic}")]
        public async Task<IActionResult> GetAvailableDoctorsForClinic(long clinic)
        {
            var response = await patientVisitService.GetAvailableDoctorsForClinic(clinic);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("get-available-dcotors-for-treatments")]
        public async Task<IActionResult> GetAvailableDoctorsForTreatment(TreatmentIdsRequest request)
        {
            var response = await patientVisitService.GetAvailableDoctorsForTreatments(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        [HttpGet("list/{isActive}")]
        public async Task<IActionResult> GetAll(bool isActive = true)
        {
            var response = await patientVisitService.GetAll(isActive);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("check-account/{contact}")]
        public async Task<IActionResult> CheckAccount(string contact)
        {
            var response = await patientVisitService.CheckPatientAccount(contact);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await patientVisitService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get-patient-visits-for-doctor")]
        public async Task<IActionResult> GetPatientVisitListForDoctor()
        {
            var response = await patientVisitService.GetPatientVisitListForDoctor();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get-appointment-history")]
        public async Task<IActionResult> GetAppointmentHistory()
        {
            var response = await patientVisitService.GetAppointmentHistory();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }


        [Authorize]
        [HttpGet("get-by-patient/{patientId}")]
        public async Task<IActionResult> GetPatientVisitsByPatientId(long patientId)
        {
            var response = await patientVisitService.GetPatientVisitsByPatientId(patientId);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get-patient-timeline/{patientId}")]
        public async Task<IActionResult> GetPatientTimeline(long patientId) 
        {
            
            var response = await patientVisitService.GetPatientTimeline(patientId);

            
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

    }
}

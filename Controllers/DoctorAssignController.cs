using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DoctorAssignRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.DoctorAssign;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/doctor-assign")]
    [ApiController]
    public class DoctorAssignController : ControllerBase
    {
        private readonly IDoctorAssignService doctorAssignService;
        public DoctorAssignController(IDoctorAssignService doctorAssignService)
        {
            this.doctorAssignService = doctorAssignService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateDoctorAssignRequest request)
        {
            var response = await doctorAssignService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, CreateDoctorAssignRequest request)
        {
            var response = await doctorAssignService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await doctorAssignService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await doctorAssignService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await doctorAssignService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) 
            BaseResponse response = await doctorAssignService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await doctorAssignService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
    }
}

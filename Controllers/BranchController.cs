using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.Branch;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Harendra_Ayurveda_HMS_WEB_API.Service.Branch;

using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/branch")]
    [ApiController]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(BranchRequest request)
        {
            var response = await _branchService.Create(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, BranchRequest request)
        {
            var response = await _branchService.Update(id, request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _branchService.GetById(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _branchService.GetAll();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _branchService.Delete(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> ids)
        {
            // Call the service to delete (soft delete) 
            BaseResponse response = await _branchService.DeleteAll(ids);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            BaseResponse response = await _branchService.UpdateStatus(id);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterRequest request)
        {
            BaseResponse response = await _branchService.Filter(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

    }

}


using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Drug;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/drug")]
    [ApiController]
    public class DrugController : Controller
    {
        private readonly IDrugService _drugService;

        public DrugController(IDrugService drugService)
        {
            _drugService = drugService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AddDrugRequest request)
        {
            var response = await _drugService.Create(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPut("update/{id}")]

        public async Task<IActionResult> Update(int id, AddDrugRequest request)
        {
            var response = await _drugService.Update(id, request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpGet("get/{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var response = await _drugService.GetById(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }


        [Authorize]
        [HttpGet("list")]

        public async Task<IActionResult> GetAll()
        {
            var response = await _drugService.GetAll();
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }


        [Authorize]
        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _drugService.Delete(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPost("delete-all")]

        public async Task<IActionResult> DeleteAll(List<int> drugIds)
        {
            var response = await _drugService.DeleteAll(drugIds);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }


        [Authorize]
        [HttpGet("update-status/{id}")]

        public async Task<IActionResult> UpdateStatus(int id)
        {
            var response = await _drugService.UpdateStatus(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }


        [Authorize]
        [HttpPost("filter")]

        public async Task<IActionResult> Filter(FilterRequestDrug request)
        {
            var response = await _drugService.Filter(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPost("import-from-excel")]
        public async Task<IActionResult> ImportDrugsFromCsv(DrugImportFromExcelRequest request)
        {
            var response = await _drugService.ImportDrugsFromCsv(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };



        }
    }
}
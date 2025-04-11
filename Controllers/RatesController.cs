using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.DrugCategoryRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatesRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.Rates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly IRatesService _ratesService;

        public RatesController(IRatesService ratesService)
        {
            this._ratesService = ratesService;
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update(RatesUpdateRequest request)
        {
            var response = await _ratesService.UpdateRates(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetRatesRecord()
        {
            var response = await _ratesService.GetRates();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
    }
}

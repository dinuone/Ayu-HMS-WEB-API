using Harendra_Ayurveda_HMS_WEB_API.Service.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService locationService;
        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var response = await locationService.GetProvinces();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [HttpGet("districts/{provinceId}")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            var response = await locationService.GetDistricts(provinceId);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        [HttpGet("cities/{districtId}")]
        public async Task<IActionResult> GetCities(int districtId)
        {
            var response = await locationService.GetCities(districtId);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
    }
}

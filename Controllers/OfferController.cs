using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.FilterRequest;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.OffersRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Offers;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/offer")]
    [ApiController]
    public class OfferController : Controller
    {
        private readonly IOffersService  _offerService;
        public OfferController(IOffersService offerService)
        {
            _offerService = offerService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AddOffersRequest request)
        {
            var response = await _offerService.Create(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, AddOffersRequest request)
        {
            var response = await _offerService.Update(id, request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _offerService.GetById(id);
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
            var response = await _offerService.GetAll(isActive);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }

        
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _offerService.Delete(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPost("delete-all")]
        public async Task<IActionResult> DeleteAll(List<int> OfferIds)
        {
            var response = await _offerService.DeleteAll(OfferIds);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpGet("update-status/{id}")]

        public async Task<IActionResult> UpdateStatus(int id)
        {
            var response = await _offerService.UpdateStatus(id);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [Authorize]
        [HttpPost("filter")]

        public async Task<IActionResult> Filter([FromBody] FilterRequest request)
        {
            var response = await _offerService.Filter(request);
            return new JsonResult(response)
            {
                StatusCode = response.StatusCode
            };




        }
    }
}

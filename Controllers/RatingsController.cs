using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatingsRequest;
using Harendra_Ayurveda_HMS_WEB_API.Service.RatingsService;
using Microsoft.AspNetCore.Mvc;

namespace Harendra_Ayurveda_HMS_WEB_API.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : Controller
    {
        private readonly IRatingService _ratingService;
        public RatingsController(IRatingService ratingService)
        {
            this._ratingService = ratingService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddRating([FromBody] AddRatingRequest request)
        {
            var response = await _ratingService.AddRating(request);
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [HttpGet("total-rating-count")]
        public async Task<IActionResult> GetTotalRatingCount()
        {
            var response = await _ratingService.GetTotalRatingCount();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
        [HttpGet("rate-count-by-rating")]
        public async Task<IActionResult> GetRateCountByOfRating()
        {
            var response = await _ratingService.GetrateCountByOfRating();
            return new JsonResult(response) { StatusCode = response.StatusCode };
        }
    }
}

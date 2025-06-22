using System.Net;
using Harendra_Ayurveda_HMS_WEB_API.DTOs.Request.RatingsRequest;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.RatingsService
{
    public class RatingService : IRatingService
    {

        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<IRatingService> logger;

        public RatingService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<IRatingService> logger)
        {
            _context = context;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }


        public async Task<BaseResponse> AddRating(AddRatingRequest request)
        {
            try
            {
                if (request.Stars < 1 || request.Stars > 5)
                {
                    return BaseResponse.CreateResponse("Rating must be between 1 and 5 stars.", HttpStatusCode.BadRequest);
                }

                var rating = new RatingsModel
                {
                    Id = request.Id,
                    Stars = request.Stars,
                    IsActive = true,
                    IsDelete = false,
                    CreatedBy = "system", 
                    CreatedAt = DateTime.UtcNow
                };

                _context.Ratings.Add(rating);
                await _context.SaveChangesAsync();

                return BaseResponse.CreateResponse("Rating added successfully.", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding rating.");
                return BaseResponse.CreateResponse("An error occurred while adding the rating.", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse> GetrateCountByOfRating()
        {
            try
            {
                // Group by Stars and count each group
                var ratingCounts = await _context.Ratings
                    .Where(r => !r.IsDelete)
                    .GroupBy(r => r.Stars)
                    .Select(g => new { Stars = g.Key, Count = g.Count() })
                    .ToListAsync();

                // Ensure all star values (1-5) are present in the result
                var result = Enumerable.Range(1, 5)
                    .Select(star => new
                    {
                        Stars = star,
                        Count = ratingCounts.FirstOrDefault(rc => rc.Stars == star)?.Count ?? 0
                    })
                    .ToList();

                return BaseResponse.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting rating counts by stars.");
                return BaseResponse.CreateResponse("An error occurred while fetching rating counts.", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse> GetTotalRatingCount()
        {
            try
            {
              
                var totalRaters = await _context.Ratings
                    .Where(r => !r.IsDelete)
                    .Distinct()
                    .CountAsync();

                return BaseResponse.CreateResponse(totalRaters, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting total rating count.");
                return BaseResponse.CreateResponse("An error occurred while fetching total rating count.", HttpStatusCode.InternalServerError);
            }
        }
    }
}
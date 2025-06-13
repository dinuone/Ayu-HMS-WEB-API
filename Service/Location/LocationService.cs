using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models.Location;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.Location
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<LocationService> logger;
        private readonly TimeZoneInfo localTimeZone;

        public LocationService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogger<LocationService> logger)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            localTimeZone = TimeZoneInfo.Local;
        }

        public async Task<BaseResponse> GetProvinces()
        {
            try
            {
                var provinces = await _context.Provinces.ToListAsync();

                if (provinces == null || !provinces.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = 404,
                        Data = new { message = "No provinces found." }
                    };
                }

                return new BaseResponse
                {
                    StatusCode = 200,
                    Data = provinces
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting provinces");
                return new BaseResponse
                {
                    StatusCode = 500,
                    Data = new { message = "An error occurred while fetching provinces." }
                };
            }
        }

        public async Task<BaseResponse> GetDistricts(int provinceId)
        {
            try
            {
                var districts = await _context.Districts
                    .Where(d => d.ProvinceId == provinceId)
                    .ToListAsync();

                if (!districts.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = 404,
                        Data = new { message = "No districts found for the specified province." }
                    };
                }

                return new BaseResponse
                {
                    StatusCode = 200,
                    Data = districts
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting districts for ProvinceId: {provinceId}", provinceId);
                return new BaseResponse
                {
                    StatusCode = 500,
                    Data = new { message = "An error occurred while fetching districts." }
                };
            }
        }

        public async Task<BaseResponse> GetCities(int districtId)
        {
            try
            {
                var cities = await _context.Cities
                    .Where(c => c.DistrictId == districtId)
                    .ToListAsync();

                if (!cities.Any())
                {
                    return new BaseResponse
                    {
                        StatusCode = 404,
                        Data = new { message = "No cities found for the specified district." }
                    };
                }

                return new BaseResponse
                {
                    StatusCode = 200,
                    Data = cities
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting cities for DistrictId: {districtId}", districtId);
                return new BaseResponse
                {
                    StatusCode = 500,
                    Data = new { message = "An error occurred while fetching cities." }
                };
            }
        }

        public async Task CheckCityExist(string districtName, string name)
        {
            try
            {
                DistrictModel? district = await _context.Districts.FirstOrDefaultAsync(d => d.Name.Equals(districtName, StringComparison.OrdinalIgnoreCase));

                if(district != null)
                {
                    CityModel? city = await _context.Cities.FirstOrDefaultAsync(c => c.DistrictId == district.Id && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (city == null)
                    {
                        city = new CityModel
                        {
                            DistrictId = district.Id,
                            Name = name
                        };

                        await _context.Cities.AddAsync(city);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while checking if the city exists for DistrictId");
                throw;
            }
        }
    }
}

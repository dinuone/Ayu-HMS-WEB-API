using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models.Location;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Seeds
{
    public class LocationSeed : IDbSeeder
    {
        public void Initialize(DbContext dbContext)
        {
            var context = (ApplicationDBContext)dbContext;

            if (!context.Provinces.Any())
            {
                var provinces = new List<ProvinceModel>
                {
                    new() { Name = "Western" },
                    new() { Name = "Central" },
                    new() { Name = "Southern" },
                    new() { Name = "Eastern" },
                    new() { Name = "North Western" },
                    new() { Name = "North Central" },
                    new() { Name = "Uva" },
                    new() { Name = "Sabaragamuwa" }
                };
                context.Provinces.AddRange(provinces);
                context.SaveChanges();
            }

            if (!context.Districts.Any())
            {
                var districts = new List<DistrictModel>
                {
                    new() { Name = "Colombo", ProvinceId = 1 },
                    new() { Name = "Gampaha", ProvinceId = 1 },
                    new() { Name = "Kalutara", ProvinceId = 1 },
                    new() { Name = "Kandy", ProvinceId = 2 },
                    new() { Name = "Matale", ProvinceId = 2 },
                    new() { Name = "Nuwara Eliya", ProvinceId = 2 },
                    new() { Name = "Galle", ProvinceId = 3 },
                    new() { Name = "Matara", ProvinceId = 3 },
                    new() { Name = "Hambantota", ProvinceId = 3 },
                    new() { Name = "Jaffna", ProvinceId = 4 },
                    new() { Name = "Kilinochchi", ProvinceId = 4 },
                    new() { Name = "Mannar", ProvinceId = 4 },
                    new() { Name = "Mullaitivu", ProvinceId = 4 },
                    new() { Name = "Vavuniya", ProvinceId = 4 },
                    new() { Name = "Trincomalee", ProvinceId = 5 },
                    new() { Name = "Batticaloa", ProvinceId = 5 },
                    new() { Name = "Ampara", ProvinceId = 5 },
                    new() { Name = "Kurunegala", ProvinceId = 6 },
                    new() { Name = "Puttalam", ProvinceId = 6 },
                    new() { Name = "Anuradhapura", ProvinceId = 7 },
                    new() { Name = "Polonnaruwa", ProvinceId = 7 },
                    new() { Name = "Badulla", ProvinceId = 8 },
                    new() { Name = "Moneragala", ProvinceId = 8 },
                    new() { Name = "Rathnapura", ProvinceId = 9 },
                    new() { Name = "Kegalle", ProvinceId = 9 }
                };
                context.Districts.AddRange(districts);
                context.SaveChanges();
            }

            if (!context.Cities.Any())
            {
                var cities = new List<CityModel>
                {
                    new() { Name = "Colombo", DistrictId = 1 },
                    new() { Name = "Dehiwala-Mount Lavinia", DistrictId = 1 },
                    new() { Name = "Moratuwa", DistrictId = 1 },
                    new() { Name = "Kotte", DistrictId = 1 },
                    new() { Name = "Nugegoda", DistrictId = 1 },
                    new() { Name = "Negombo", DistrictId = 2 },
                    new() { Name = "Gampaha", DistrictId = 2 },
                    new() { Name = "Wattala", DistrictId = 2 },
                    new() { Name = "Katunayake", DistrictId = 2 },
                    new() { Name = "Kalutara", DistrictId = 3 },
                    new() { Name = "Panadura", DistrictId = 3 },
                    new() { Name = "Kandy", DistrictId = 4 },
                    new() { Name = "Peradeniya", DistrictId = 4 },
                    new() { Name = "Gampola", DistrictId = 4 },
                    new() { Name = "Matale", DistrictId = 5 },
                    new() { Name = "Dambulla", DistrictId = 5 },
                    new() { Name = "Nuwara Eliya", DistrictId = 6 },
                    new() { Name = "Hatton", DistrictId = 6 },
                    new() { Name = "Galle", DistrictId = 7 },
                    new() { Name = "Ambalangoda", DistrictId = 7 },
                    new() { Name = "Matara", DistrictId = 8 },
                    new() { Name = "Weligama", DistrictId = 8 },
                    new() { Name = "Hambantota", DistrictId = 9 },
                    new() { Name = "Tangalle", DistrictId = 9 },
                    new() { Name = "Jaffna", DistrictId = 10 },
                    new() { Name = "Nallur", DistrictId = 10 },
                    new() { Name = "Kilinochchi", DistrictId = 11 },
                    new() { Name = "Mannar", DistrictId = 12 },
                    new() { Name = "Mullaitivu", DistrictId = 13 },
                    new() { Name = "Vavuniya", DistrictId = 14 },
                    new() { Name = "Trincomalee", DistrictId = 15 }
                };
                context.Cities.AddRange(cities);
                context.SaveChanges();
            }
        }
    }
}

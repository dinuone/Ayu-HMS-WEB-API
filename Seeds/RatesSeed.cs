using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Seeds
{
    public class RatesSeed : IDbSeeder
    {
        public void Initialize(DbContext dbContext)
        {
            var applicationDbContext = (ApplicationDBContext)dbContext;

            // Check if a Rates record exists
            if (!applicationDbContext.Rates.Any())
            {
                var rates = new RatesModel
                {
                    HospitalCharge = 0m,
                    CreatedBy = "system"
                };

                applicationDbContext.Rates.Add(rates);
                applicationDbContext.SaveChanges();
            }
        }
    }
}

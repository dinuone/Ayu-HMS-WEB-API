using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Seeds
{
    public class BranchSeed : IDbSeeder
    {

        public void Initialize(DbContext dbContext)
        {
            var applicationDbContext = (ApplicationDBContext)dbContext;

            if (!applicationDbContext.Branches.Any())
            {
                var branches = new List<BranchModel>
                {
                    new BranchModel
                    {
                        Name = "Ragama",
                        Address = "ragama",
                        CreatedBy = "system",
                        PhoneNumber = "1234567890",
                    },

                    new BranchModel
                    {
                        Name = "Kaduwela",
                        Address = "ragama",
                        CreatedBy = "system",
                        PhoneNumber = "1234567890",
                    },
                 };


                applicationDbContext.Branches.AddRange(branches);
                applicationDbContext.SaveChanges();
            }
        }
    }
}

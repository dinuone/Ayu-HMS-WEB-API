using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API.Seeds
{
    public class RoleSeed : IDbSeeder
    {
        public void Initialize(DbContext dbContext)
        {
            var applicationDbContext = (ApplicationDBContext)dbContext;

            if (!applicationDbContext.Roles.Any())
            {
                var roles = new List<RoleModel>
                {
                    new RoleModel
                    {
                        Name = "SuperAdmin",
                    },
                     new RoleModel
                    {
                        Name = "Branch Admin",
                    },
                     new RoleModel
                    {
                        Name = "Doctor",
                    },
                       new RoleModel
                    {
                        Name = "Coordinator",
                    },

                     new RoleModel
                    {
                        Name = "MOI",
                    },

                 };

                applicationDbContext.Roles.AddRange(roles);
                applicationDbContext.SaveChanges();
            }
        }
    }
}

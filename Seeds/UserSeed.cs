using Harendra_Ayurveda_HMS_WEB_API;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.Seeds
{
    public class UserSeed : IDbSeeder
    {
        public void Initialize(DbContext dbContext)
        {
            var applicationDbContext = (ApplicationDBContext)dbContext;

            if (!applicationDbContext.Users.Any())
            {
                // no records found. then seeding
                // define objects
                string hashPassword = BCrypt.Net.BCrypt.HashPassword("123456");

                var users = new List<UserModel>
                {
                   new UserModel
                    {
                        Username = "admin",
                        Password = hashPassword,
                        Email = "admin@gmail.com",
                        Role = 1,
                        IsActive = true,
                        FirstName = "super",
                        LastName = "admin",
                        CreatedBy = "system"
                    },

                 };


                applicationDbContext.Users.AddRange(users);
                applicationDbContext.SaveChanges();
            }
        }
    }
}

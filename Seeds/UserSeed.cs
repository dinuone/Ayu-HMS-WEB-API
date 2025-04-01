using BookInventory.Helpers;
using BookInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.Seeds
{
    public class RoleSeed : IDbSeeder
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
                        Username ="admin",
                        Password = hashPassword,
                        Email = "admin@gmail.com",
                        Role = "Admin",
                        isActive = true,
                        FirstName = "super",
                        LastName = "admin",
                    },

                    new UserModel
                    {
                        Username ="user123",
                        Password = hashPassword,
                        Email = "user@gmail.com",
                        Role = "Cashier",
                        isActive = true,
                        FirstName = "test",
                        LastName = "user",
                    },
                 };


                applicationDbContext.Users.AddRange(users);
                applicationDbContext.SaveChanges();
            }
        }
    }
}

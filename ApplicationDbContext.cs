using BookInventory.Helpers;
using BookInventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookInventory
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
          : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseMySql(GlobalAttributeHelper.mySqlConfiguration.connectionString,
                   GlobalAttributeHelper.mySqlConfiguration.dbVersion);
            }
        }

      

    }
}

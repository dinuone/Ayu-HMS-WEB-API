using BookInventory.Models;
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Harendra_Ayurveda_HMS_WEB_API
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
        public DbSet<BranchModel> Branches { get; set; }
        public DbSet <TreatmentCategoryModel> TreatmentCategories { get; set; }
        public DbSet<UserBranchModel> UserBranches { get; set; }
        public DbSet<ClinicCategoryModel> ClinicCategories { get; set; }
        public DbSet <DiseaseCodeManagementModel> DiseaseCodes { get; set; }

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

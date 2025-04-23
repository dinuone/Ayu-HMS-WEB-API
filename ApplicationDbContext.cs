
using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Models;
using Harendra_Ayurveda_HMS_WEB_API.Models.Location;
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

        public DbSet <UserModel> Users { get; set; }
        public DbSet <BranchModel> Branches { get; set; }
        public DbSet <TreatmentCategoryModel> TreatmentCategories { get; set; }
        public DbSet<UserBranchModel> UserBranches { get; set; }
        public DbSet <ClinicCategoryModel> ClinicCategories { get; set; }
        public DbSet <TreatmentModel> Treatments { get; set; }
        public DbSet <DiseaseCodeModel> DiseaseCodes { get; set; }
        public DbSet <DrugCategoryModel> DrugCategories { get; set; }
        public DbSet<DrugModel> Drugs { get; set; }
        public DbSet <PatientModel> Patients { get; set; }
        public DbSet <RoleModel> Roles { get; set; }
        public DbSet <RatesModel> Rates { get; set; }
        public DbSet <SequenceDataModel> Sequences { get; set; }
        public DbSet <OffersModel> Offers { get; set; }
        public DbSet <CityModel> Cities { get; set; }
        public DbSet <ProvinceModel> Provinces { get; set; }
        public DbSet <DistrictModel> Districts { get; set; }

        public DbSet<PatientVisitModel> patientVisits { get; set; }

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

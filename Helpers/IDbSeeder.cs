using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BookInventory.Helpers
{
    public interface IDbSeeder
    {
        void Initialize(DbContext dbContext);
    }

    public static class DbSeeder
    {
        public static void Seed(Assembly assembly, DbContext dbContext)
        {
            var seeders = assembly.GetTypes()
                .Where(t => typeof(IDbSeeder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IDbSeeder>();

            foreach (var seeder in seeders)
            {
                seeder.Initialize(dbContext);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace BookInventory.Helpers
{
    public static class DBFunctions
    {
        private static List<string> dbFunctions;
        public static WebApplication CreateDatabase(this WebApplication webApplication, DbContext context)
        {
            // Check if dbFunctions is not empty before executing
            if (dbFunctions == null || dbFunctions.Count == 0)
            {
                Console.WriteLine("No functions to create.");
                return webApplication;
            }

            // Open the database connection
            context.Database.OpenConnection();

            var connection = context.Database.GetDbConnection();

            foreach (var func in dbFunctions)
            {
                using var command = connection.CreateCommand();
                command.CommandText = func;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to execute function: {func}. Error: {ex.Message}");
                }
            }

            context.Database.CloseConnection();

            return webApplication;
        }

        // Add a method to populate functions
        public static void AddFunction(string functionDefinition)
        {
            if (!string.IsNullOrEmpty(functionDefinition))
            {
                dbFunctions.Add(functionDefinition);
            }
        }
    }
}

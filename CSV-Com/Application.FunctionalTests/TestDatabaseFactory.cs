using Microsoft.EntityFrameworkCore;

namespace Application.FunctionalTests
{
    public static class TestDatabaseFactory<TContext> where TContext : DbContext
    {
        public static async Task<ITestDatabase> CreateAsync(string connectionString)
        {
#if DEBUG
            var database = new MySqlTestDatabase<TContext>(connectionString);
#else
        var database = new TestcontainersTestDatabase();
#endif
            await database.InitialiseAsync();

            return database;
        }
    }
}

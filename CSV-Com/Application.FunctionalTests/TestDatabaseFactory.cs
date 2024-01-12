namespace Application.FunctionalTests
{
    public static class TestDatabaseFactory
    {
        public static async Task<ITestDatabase> CreateAsync(string connectionString)
        {
#if DEBUG
            var database = new MySqlTestDatabase(connectionString);
#else
        var database = new TestcontainersTestDatabase();
#endif
            await database.InitialiseAsync();

            return database;
        }
    }
}

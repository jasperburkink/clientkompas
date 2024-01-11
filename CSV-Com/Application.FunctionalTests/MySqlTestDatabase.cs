using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;

namespace Application.FunctionalTests
{
    public class MySqlTestDatabase : ITestDatabase
    {
        private readonly string _connectionString = null!;
        private SqlConnection _connection = null!;
        private Respawner _respawner = null!;

        public MySqlTestDatabase(string connectionString)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Guard.Against.Null(connectionString);

            _connectionString = connectionString;
        }

        public async Task InitialiseAsync()
        {
            _connection = new SqlConnection(_connectionString);

            var options = new DbContextOptionsBuilder<DbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            var context = new DbContext(options);

            context.Database.Migrate();

            _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
            });
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }

        public async Task ResetAsync()
        {
            await _respawner.ResetAsync(_connectionString);
        }

        public async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}

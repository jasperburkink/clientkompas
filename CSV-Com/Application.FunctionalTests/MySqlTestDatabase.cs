using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Respawn;

namespace Application.FunctionalTests
{
    public class MySqlTestDatabase<TContext> : ITestDatabase where TContext : DbContext
    {
        private readonly string _connectionString = null!;
        private MySqlConnection _connection = null!;
        private Respawner _respawner = null!;

        public MySqlTestDatabase(string connectionString)
        {
            Guard.Against.Null(connectionString);

            _connectionString = connectionString;
        }

        public async Task InitialiseAsync()
        {
            var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder(_connectionString)
            {
                AllowUserVariables = true,
                UseAffectedRows = false
            };

            _connection = new MySqlConnection(mySqlConnectionStringBuilder.ConnectionString);

            var options = new DbContextOptionsBuilder<TContext>()
                .UseMySql(_connectionString, MySqlServerVersion.LatestSupportedServerVersion)
                .Options;

            var context = (DbContext)Activator.CreateInstance(typeof(TContext), options);

            context.Database.Migrate();

            _connection.Open();

            _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.MySql,
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }

        public async Task ResetAsync()
        {
            await _respawner.ResetAsync(_connection);
        }

        public async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}

﻿using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.MySql;

namespace Application.FunctionalTests
{
    public class TestcontainersTestDatabase : ITestDatabase
    {
        private readonly MySqlContainer _container;
        private DbConnection _connection = null!;
        private string _connectionString = null!;
        private Respawner _respawner = null!;

        public TestcontainersTestDatabase()
        {
            _container = new MySqlBuilder()
                .WithAutoRemove(true)
                .Build();
        }

        public async Task InitialiseAsync()
        {
            await _container.StartAsync();

            _connectionString = _container.GetConnectionString();

            _connection = new SqlConnection(_connectionString);

            var options = new DbContextOptionsBuilder<DbContext>()
                .UseMySql(_connectionString, MySqlServerVersion.LatestSupportedServerVersion)
                .Options;

            var context = new DbContext(options);

            context.Database.Migrate();

            _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
            {
                TablesToIgnore = ["__EFMigrationsHistory"]
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
            await _container.DisposeAsync();
        }

        public async Task DropAsync()
        {
            await _container.StopAsync();
        }
    }
}

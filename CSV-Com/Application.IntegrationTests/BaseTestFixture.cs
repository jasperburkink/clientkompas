using System.Data.Common;
using Infrastructure.Identity;
using Infrastructure.Persistence.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Respawn;

namespace Application.IntegrationTests
{
    public class BaseTestFixture : IDisposable
    {
        public WebApplicationFactory<Program> Factory { get; init; } = null!;
        public IConfiguration Configuration { get; init; } = null!;
        public IServiceScopeFactory ScopeFactory { get; init; } = null!;
        public Respawner Checkpoint { get; init; } = null!;

        private string? _currentUserId;

        public BaseTestFixture()
        {
            Factory = new CustomWebApplicationFactory(_currentUserId);
            ScopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();
            Configuration = Factory.Services.GetRequiredService<IConfiguration>();

            DbConnection cmd = new MySqlConnection(Configuration.GetConnectionString("AuthenticationConnectionString"));
            cmd.Open();

            // TODO: Error is thrown cannot connect with the database because: Table 'sys.tables' doesn't exist'
            Checkpoint = Respawner.CreateAsync(cmd, new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
            }).GetAwaiter().GetResult();

            ResetState().GetAwaiter().GetResult();
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ScopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public async Task SendAsync(IBaseRequest request)
        {
            using var scope = ScopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            await mediator.Send(request);
        }

        public string? GetCurrentUserId()
        {
            return _currentUserId;
        }

        public async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
        }

        public async Task<string> RunAsAdministratorAsync()
        {
            return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
        }

        public async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
            using var scope = ScopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser { UserName = userName, Email = userName };

            var result = await userManager.CreateAsync(user, password);

            if (roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRolesAsync(user, roles);
            }

            if (result.Succeeded)
            {
                _currentUserId = user.Id;

                return _currentUserId;
            }

            var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

        public async Task ResetState()
        {
            try
            {
                await Checkpoint.ResetAsync(Configuration.GetConnectionString("AuthenticationConnectionString")!);
                //await _fixture.Checkpoint.ResetAsync(_fixture.Configuration.GetConnectionString("DefaultConnection")!);
            }
            catch (Exception)
            {
            }

            _currentUserId = null;
        }

        public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            return await context.Set<TEntity>().CountAsync();
        }

        public void Dispose()
        {

        }
    }
}

using System.Linq.Expressions;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.FunctionalTests
{
    [SetUpFixture]
    public partial class Testing
    {
        private const int DATABASE_PREFIX_LENGTH = 10;

        private static ITestDatabase s_databaseCSV;
        private static ITestDatabase s_databaseAuthentication;
        private static CustomWebApplicationFactory s_factory = null!;
        private static CustomWebApplicationFactoryWithMocks s_factoryWithMocks = null!;
        private static IServiceScopeFactory s_scopeFactory = null!, s_scopeFactoryWithMocks = null!;
        private static string? s_currentUserId;
        private static readonly string? s_databasePrefix = GenerateRandomPrefix();
        public static bool UseMocks { get; set; } = false;
        public static IIdentityService IdentityService => CreateScope().ServiceProvider.GetRequiredService<IIdentityService>();

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            // Configuration + Connectionstrings
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Temporary databases for testing
            var authenticationConnectionString = configuration.GetConnectionString("AuthenticationConnectionString")
                ?.Replace("<<test_db_name>>", $"{s_databasePrefix}_Authentication");

            var csvConnectionString = configuration.GetConnectionString("CVSConnectionString")
                ?.Replace("<<test_db_name>>", $"{s_databasePrefix}_CVS");

            s_databaseAuthentication = await TestDatabaseFactory<AuthenticationDbContext>.CreateAsync(authenticationConnectionString!);
            s_databaseCSV = await TestDatabaseFactory<CVSDbContext>.CreateAsync(csvConnectionString!);

            var connectionCvs = s_databaseCSV.GetConnection();
            var connectionAuthentication = s_databaseAuthentication.GetConnection();

            s_factory = new CustomWebApplicationFactory(connectionCvs, connectionAuthentication);
            s_scopeFactory = s_factory.Services.GetRequiredService<IServiceScopeFactory>();

            s_factoryWithMocks = new CustomWebApplicationFactoryWithMocks(connectionCvs, connectionAuthentication);
            s_scopeFactoryWithMocks = s_factoryWithMocks.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public static async Task SendAsync(IBaseRequest request)
        {
            using var scope = CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            await mediator.Send(request);
        }

        public static string? GetCurrentUserId()
        {
            return s_currentUserId;
        }

        public static async Task<string> RunAsAsync(string role)
        {
            return await RunAsUserAsync(role, $"{role}1!", role);
        }

        public static async Task<string> RunAsUserAsync(string userName, string password, string role, string id = "")
        {
            using var scope = CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthenticationUser>>();

            var user = new AuthenticationUser
            {
                Id = id,
                UserName = userName,
                Email = userName
            };

            var result = await userManager.CreateAsync(user, password);

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(role));

            await userManager.AddToRolesAsync(user, [role]);

            if (result.Succeeded)
            {
                s_currentUserId = user.Id;

                return s_currentUserId;
            }

            var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

        public static async Task ResetState()
        {
            try
            {
                await s_databaseCSV.ResetAsync();
            }
            catch (Exception)
            {
            }

            s_currentUserId = null;
        }

        public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            return await FindAsync<TEntity, CVSDbContext>(keyValues);
        }

        public static async Task<TEntity?> FindAsync<TEntity, TDbContext>(params object[] keyValues)
            where TEntity : class
            where TDbContext : DbContext
        {
            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static async Task<ICollection<TEntity>> GetAsync<TEntity>(params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class
        {
            return await GetAsync<TEntity, CVSDbContext>(includes);
        }

        public static async Task<ICollection<TEntity>> GetAsync<TEntity, TDbContext>(params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class
            where TDbContext : DbContext
        {
            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            IQueryable<TEntity> query = context.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            await AddAsync<TEntity, CVSDbContext>(entity);
        }

        public static async Task AddAsync<TEntity, TDbContext>(TEntity entity)
            where TEntity : class
            where TDbContext : DbContext
        {
            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = s_scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CVSDbContext>();

            context.Update(entity);

            await context.SaveChangesAsync();
        }

        public static async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            using var scope = CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CVSDbContext>();

            return await context.Set<TEntity>().CountAsync();
        }

        public static async Task<string> CreateUserAsync(string userName, string password)
        {
            using var scope = CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthenticationUser>>();


            var user = new AuthenticationUser
            {
                UserName = userName,
                Email = userName
            };

            var result = await userManager.CreateAsync(user, password);

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (result.Succeeded)
            {
                s_currentUserId = user.Id;

                return s_currentUserId;
            }

            var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }

        public static async Task<string> GetPasswordResetTokenAsync(AuthenticationUser user)
        {
            using var scope = CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthenticationUser>>();

            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        private static IServiceScope CreateScope()
        {
            return UseMocks ? s_scopeFactoryWithMocks.CreateScope() : s_scopeFactory.CreateScope();
        }

        [OneTimeTearDown]
        public async Task RunAfterAnyTests()
        {
            await s_databaseAuthentication.DropAsync();
            await s_databaseCSV.DropAsync();
            await s_databaseCSV.DisposeAsync();
            await s_factory.DisposeAsync();
            await s_factory.DisposeAsync();
        }

        private static string GenerateRandomPrefix()
        {
            var characters = "abcdefghijklmnopqrstuvwxyz1234567890";
            var prefix = "test_";
            for (var i = 0; i < DATABASE_PREFIX_LENGTH; i++)
            {
                prefix += characters[Random.Shared.Next(0, characters.Length)];
            }
            return prefix;
        }

        [OneTimeTearDown]
        public void OnTearDown()
        {
            s_factoryWithMocks?.Dispose();
        }
    }
}

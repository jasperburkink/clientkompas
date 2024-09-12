using System.Linq.Expressions;
using Domain.Authentication.Constants;
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
        private static IServiceScopeFactory s_scopeFactory = null!;
        private static string? s_userId;
        private static readonly string? s_databasePrefix = GenerateRandomPrefix();

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            // Stel de configuratie in
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var authenticationConnectionString = configuration.GetConnectionString("AuthenticationConnectionString")
                ?.Replace("<<test_db_name>>", $"{s_databasePrefix}_Authentication");

            var csvConnectionString = configuration.GetConnectionString("CVSConnectionString")
                ?.Replace("<<test_db_name>>", $"{s_databasePrefix}_CVS");

            s_databaseAuthentication = await TestDatabaseFactory<AuthenticationDbContext>.CreateAsync(authenticationConnectionString!);
            s_databaseCSV = await TestDatabaseFactory<CVSDbContext>.CreateAsync(csvConnectionString!);

            var connectionCvs = s_databaseCSV.GetConnection();
            var connectionAuthentication = s_databaseAuthentication.GetConnection();

            // Configureer andere vereiste diensten en instanties
            s_factory = new CustomWebApplicationFactory(connectionCvs, connectionAuthentication);
            s_scopeFactory = s_factory.Services.GetRequiredService<IServiceScopeFactory>();
        }


        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = s_scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }

        public static async Task SendAsync(IBaseRequest request)
        {
            using var scope = s_scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            await mediator.Send(request);
        }

        public static string? GetUserId()
        {
            return s_userId;
        }

        public static async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local", "Testing1234!", []);
        }

        public static async Task<string> RunAsAdministratorAsync()
        {
            return await RunAsUserAsync("administrator@local", "Administrator1234!", [Roles.Administrator]);
        }

        public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
            using var scope = s_scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthenticationUser>>();

            var hasher = new Argon2Hasher();
            var salt = hasher.GenerateSalt();
            var passwordHash = hasher.HashPassword(password, salt);

            var user = new AuthenticationUser
            {
                UserName = userName,
                Email = userName,
                Salt = salt,
                PasswordHash = passwordHash
            };

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
                s_userId = user.Id;

                return s_userId;
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

            s_userId = null;
        }

        public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            using var scope = s_scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CVSDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }

        public static async Task<ICollection<TEntity>> GetAsync<TEntity>(params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class
        {
            using var scope = s_scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CVSDbContext>();

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
            using var scope = s_scopeFactory.CreateScope();

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
            using var scope = s_scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CVSDbContext>();

            return await context.Set<TEntity>().CountAsync();
        }

        [OneTimeTearDown]
        public async Task RunAfterAnyTests()
        {
            await s_databaseAuthentication.DropAsync();
            await s_databaseCSV.DropAsync();
            await s_databaseCSV.DisposeAsync();
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
    }
}

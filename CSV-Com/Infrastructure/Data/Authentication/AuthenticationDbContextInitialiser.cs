using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Authentication
{
    public class AuthenticationDbContextInitialiser
    {
        private readonly ILogger<AuthenticationDbContextInitialiser> _logger;
        private readonly AuthenticationDbContext _context;
        private readonly UserManager<AuthenticationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationDbContextInitialiser(ILogger<AuthenticationDbContextInitialiser> logger, AuthenticationDbContext context, UserManager<AuthenticationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsMySql())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default roles
            var administratorRole = new IdentityRole(nameof(Roles.Administrator));
            await CreateRoleAndUser(administratorRole);

            var licenseeRole = new IdentityRole(nameof(Roles.Licensee));
            await CreateRoleAndUser(licenseeRole);

            var systemOwnerRole = new IdentityRole(nameof(Roles.SystemOwner));
            await CreateRoleAndUser(systemOwnerRole);

            var systemCoach = new IdentityRole(nameof(Roles.Coach));
            await CreateRoleAndUser(systemCoach);
        }

        private async Task CreateRoleAndUser(IdentityRole role)
        {
            if (_roleManager.Roles.All(r => r.Name != role.Name))
            {
                await _roleManager.CreateAsync(role);
            }

            // Default users
            var user = new AuthenticationUser { UserName = $"{role.Name}@localhost", Email = $"{role.Name}@localhost" };

            if (_userManager.Users.All(u => u.UserName != user.UserName))
            {
                await _userManager.CreateAsync(user, $"{role.Name}1!");
                if (!string.IsNullOrWhiteSpace(role.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { role.Name });
                }
            }
        }
    }
}

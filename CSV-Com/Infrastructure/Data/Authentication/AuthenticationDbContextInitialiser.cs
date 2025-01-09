using Application.Common.Interfaces.Authentication;
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
        private readonly IIdentityService _identityService;
        private const string DOMAIN_CLIENTKOMPAS = "clientkompas.nl";

        public AuthenticationDbContextInitialiser(ILogger<AuthenticationDbContextInitialiser> logger, AuthenticationDbContext context, UserManager<AuthenticationUser> userManager, RoleManager<IdentityRole> roleManager, IIdentityService identityService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityService = identityService;
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
            var password = $"{role.Name}{role.Name}1!";

            var email = $"{role}@{DOMAIN_CLIENTKOMPAS}";

            if (_userManager.Users.All(u => u.UserName != email))
            {
                var (result, userId) = await _identityService.CreateUserAsync(email, password);

                if (result == null || userId == null)
                {
                    return;
                }

                var user = await _identityService.GetUserAsync(userId);

                if (!string.IsNullOrWhiteSpace(role.Name))
                {
                    await _userManager.AddToRolesAsync(user, new[] { role.Name }); // TODO: move the addroles to the indetityservice and remove usermanager from this class
                }
            }
        }
    }
}

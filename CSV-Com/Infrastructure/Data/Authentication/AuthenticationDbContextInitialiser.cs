using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Authentication
{
    public class AuthenticationDbContextInitialiser(ILogger<AuthenticationDbContextInitialiser> logger, AuthenticationDbContext context, UserManager<AuthenticationUser> userManager, RoleManager<IdentityRole> roleManager, IIdentityService identityService)
    {
        private const string DOMAIN_CLIENTKOMPAS = "clientkompas.nl";

        public async Task InitialiseAsync()
        {
            try
            {
                if (context.Database.IsMySql())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initialising the database.");
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
                logger.LogError(ex, "An error occurred while seeding the database.");
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
            if (roleManager.Roles.All(r => r.Name != role.Name))
            {
                await roleManager.CreateAsync(role);
            }

            // Default users
            var password = $"{role.Name}{role.Name}1!";

            var email = $"{role}@{DOMAIN_CLIENTKOMPAS}";

            if (userManager.Users.All(u => u.UserName != email))
            {
                var (result, userId) = await identityService.CreateUserAsync(email, password);

                if (result == null || userId == null)
                {
                    return;
                }

                var user = await identityService.GetUserAsync(userId);

                if (!string.IsNullOrWhiteSpace(role.Name))
                {
                    await userManager.AddToRolesAsync(user, [role.Name]); // TODO: move the addroles to the indetityservice and remove usermanager from this class
                }
            }
        }
    }
}

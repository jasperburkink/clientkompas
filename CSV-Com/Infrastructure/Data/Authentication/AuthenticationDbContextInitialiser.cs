using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Authentication
{
    public class AuthenticationDbContextInitialiser(ILogger<AuthenticationDbContextInitialiser> logger, AuthenticationDbContext context,
        UserManager<AuthenticationUser> userManager, RoleManager<AuthenticationRole> roleManager, IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        private const string DOMAIN_CLIENTKOMPAS = "clientkompas.nl";
        private const string PHONENUMBER_SBICT = "0623452092";

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
            var administratorRole = new AuthenticationRole(nameof(Roles.Administrator));
            await CreateRole(administratorRole);

            var licenseeRole = new AuthenticationRole(nameof(Roles.Licensee));
            await CreateRole(licenseeRole);

            var systemOwnerRole = new AuthenticationRole(nameof(Roles.SystemOwner));
            await CreateRole(systemOwnerRole);

            var systemCoach = new AuthenticationRole(nameof(Roles.Coach));
            await CreateRole(systemCoach);

            // Create systemowner role so lincences with users can be added
            await CreateUser(systemOwnerRole.Name);
        }

        private async Task CreateRole(AuthenticationRole role)
        {
            if (roleManager.Roles.All(r => r.Name != role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }

        private async Task CreateUser(string userName)
        {
            // Default users
            var password = $"{userName}{userName}1!";

            var email = $"{userName}@{DOMAIN_CLIENTKOMPAS}";

            // CVSUser?
            var cvsUser = (await unitOfWork.UserRepository.GetAsync(u => u.EmailAddress.ToLower() == email.ToLower())).FirstOrDefault();

            if (cvsUser == null)
            {
                await unitOfWork.UserRepository.InsertAsync(new User
                {
                    EmailAddress = email,
                    FirstName = userName,
                    LastName = userName,
                    TelephoneNumber = PHONENUMBER_SBICT
                });
                await unitOfWork.SaveAsync();
                cvsUser = (await unitOfWork.UserRepository.GetAsync(u => u.EmailAddress.ToLower() == email.ToLower())).First();
            }

            if (userManager.Users.All(u => u.UserName != email))
            {
                var (result, userId) = await identityService.CreateUserAsync(email, password, cvsUser.Id);

                if (result == null || userId == null)
                {
                    return;
                }

                var user = await identityService.GetUserAsync(userId);

                if (!string.IsNullOrWhiteSpace(userName))
                {
                    await userManager.AddToRolesAsync((AuthenticationUser)user, [userName]); // TODO: move the addroles to the indetityservice and remove usermanager from this class
                }
            }
        }
    }
}

using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;
using FluentAssertions;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FunctionalTests.Identity
{
    public class IdentityServiceIntegrationTests
    {
        private readonly IdentityService _identityService;
        private readonly UserManager<AuthenticationUser> _userManager;
        private readonly RoleManager<AuthenticationRole> _roleManager;

        public IdentityServiceIntegrationTests()
        {
            var serviceCollection = new ServiceCollection()
            .AddLogging()
            .AddAuthorization()
            .AddDbContext<AuthenticationDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()))
            .AddIdentity<AuthenticationUser, AuthenticationRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

            serviceCollection.Services.AddScoped<IAuthenticationDbContext>(provider => provider.GetService<AuthenticationDbContext>());
            serviceCollection.Services.AddScoped<IHasher, Argon2Hasher>();
            serviceCollection.Services.AddScoped<ITokenService, TokenService>();
            serviceCollection.Services.AddScoped<IEmailService, EmailService>();

            // Build de ServiceProvider op de ServiceCollection, niet op de IdentityBuilder
            var serviceProvider = serviceCollection.Services.BuildServiceProvider();

            _userManager = serviceProvider.GetRequiredService<UserManager<AuthenticationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<AuthenticationRole>>();
            var authenticationDbContext = serviceProvider.GetRequiredService<IAuthenticationDbContext>();
            var signInManager = serviceProvider.GetRequiredService<SignInManager<AuthenticationUser>>();
            var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
            var hasher = serviceProvider.GetRequiredService<IHasher>();
            var refreshtokenService = serviceProvider.GetRequiredService<ITokenService>();
            var emailService = serviceProvider.GetRequiredService<IEmailService>();

            _identityService = new IdentityService(_userManager, signInManager, _roleManager, null,
                authorizationService, authenticationDbContext, hasher,
                refreshtokenService, emailService);
        }

        #region CreateUserAsync

        [Fact]
        public async Task CreateUserAsync_ValidInput_ShouldReturnSuccessResult()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            // Act
            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);

            // Assert
            result.Succeeded.Should().BeTrue();
            userId.Should().NotBeNullOrEmpty();

            var user = await _userManager.FindByIdAsync(userId);
            user.Should().NotBeNull();
            user.UserName.Should().Be(userName);
        }

        #endregion

        #region LoginAsync

        [Fact(Skip = "Login gives a known httpcontext is null error. Skip for now.")]
        public async Task LoginAsync_ValidCredentials_ShouldReturnSuccessResult()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            // Create user
            var (result, _) = await _identityService.CreateUserAsync(userName, password, 0);
            result.Succeeded.Should().BeTrue();

            // Act
            var loginResult = await _identityService.LoginAsync(userName, password);

            // Assert
            loginResult.Succeeded.Should().BeTrue();
            loginResult.User.Should().NotBeNull();
            loginResult.User.UserName.Should().Be(userName);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ShouldReturnFailedResult()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "WrongPassword";

            // Create user
            var (result, _) = await _identityService.CreateUserAsync(userName, "Password123!", 0);
            result.Succeeded.Should().BeTrue();

            // Act
            var loginResult = await _identityService.LoginAsync(userName, password);

            // Assert
            loginResult.Succeeded.Should().BeFalse();
            loginResult.User.Should().BeNull();
        }

        #endregion

        #region GetUserNameAsync

        [Fact]
        public async Task GetUserNameAsync_UserExists_ShouldReturnUserName()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            // Create user
            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            result.Succeeded.Should().BeTrue();

            // Act
            var usernameResult = await _identityService.GetUserNameAsync(userId);

            // Assert
            usernameResult.Should().Be(userName);
        }

        [Fact]
        public async Task GetUserNameAsync_UserDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = await _identityService.GetUserNameAsync("nonexistent");

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region DeleteUserAsync

        [Fact]
        public async Task DeleteUserAsync_UserExists_ShouldReturnSuccessResult()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            // Create user
            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            result.Succeeded.Should().BeTrue();

            // Act
            var deleteResult = await _identityService.DeleteUserAsync(userId);

            // Assert
            deleteResult.Succeeded.Should().BeTrue();

            var deletedUser = await _userManager.FindByIdAsync(userId);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUserAsync_UserDoesNotExist_ShouldReturnSuccessResult()
        {
            // Act
            var result = await _identityService.DeleteUserAsync("nonexistent");

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        #endregion

        #region IsInRoleAsync

        [Fact]
        public async Task IsInRoleAsync_UserIsInRole_ShouldReturnTrue()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";
            var role = Roles.Administrator;

            // Create user
            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            result.Succeeded.Should().BeTrue();

            // Assign role
            await _roleManager.CreateAsync(new AuthenticationRole(role));
            await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), role);

            // Act
            var isInRole = await _identityService.IsInRoleAsync(userId, role);

            // Assert
            isInRole.Should().BeTrue();
        }

        [Fact]
        public async Task IsInRoleAsync_UserIsNotInRole_ShouldReturnFalse()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            // Create user
            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            result.Succeeded.Should().BeTrue();

            // Act
            var isInRole = await _identityService.IsInRoleAsync(userId, "NonexistentRole");

            // Assert
            isInRole.Should().BeFalse();
        }

        #endregion

        [Fact]
        public async Task Get2FATokenAsync_CorrectFlow_ReturnToken()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);

            // Act
            var token = await _identityService.Get2FATokenAsync(userId);

            // Assert
            token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get2FATokenAsync_UserDoesNotExists_ShouldThrowNotFoundException()
        {
            // Arrange
            var userId = "Not a valid userid";

            // Act
            Func<Task<string>> act = async () => await _identityService.Get2FATokenAsync(userId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(Skip = "Login gives a known httpcontext is null error. Skip for now.")]
        public async Task Login2FAAsync_CorrectFlow_UserShouldBeLoggedIn()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            var token = await _identityService.Get2FATokenAsync(userId);

            // Act
            var resultLogin = await _identityService.Login2FAAsync(userId, token);

            // Assert
            resultLogin.Should().NotBeNull();
            resultLogin.Succeeded.Should().BeTrue();
        }

        [Fact(Skip = "Login gives a known httpcontext is null error. Skip for now.")]
        public async Task Login2FAAsync_CorrectFlow_UserShouldHaveRoles()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";
            var role = nameof(Roles.Coach);

            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            await _roleManager.CreateAsync(new AuthenticationRole(role));
            await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), role);

            var token = await _identityService.Get2FATokenAsync(userId);

            // Act
            var resultLogin = await _identityService.Login2FAAsync(userId, token);

            // Assert
            resultLogin.Should().NotBeNull();
            resultLogin.Roles.Should().NotBeEmpty().And.Contain(role);
        }

        [Fact]
        public async Task Login2FAAsync_UserDoesNotExists_ShouldThrowNotFoundException()
        {
            // Arrange
            var userId = "Not a valid user id.";
            var token = "132546";

            // Act
            Func<Task<LoggedInResult>> act = async () => await _identityService.Login2FAAsync(userId, token);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact(Skip = "Login gives a known httpcontext is null error. Skip for now.")]
        public async Task Login2FAAsync_TokenIsInvalid_UserShouldNotBeLoggedIn()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            var token = "WrongToken";

            // Act
            var resultLogin = await _identityService.Login2FAAsync(userId, token);

            // Assert
            resultLogin.Should().NotBeNull();
            resultLogin.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async Task GetUserRolesAsync_CorrectFlow_UserShouldHaveRoles()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";
            var role = nameof(Roles.Coach);

            var (result, userId) = await _identityService.CreateUserAsync(userName, password, 0);
            await _roleManager.CreateAsync(new AuthenticationRole(role));
            await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), role);

            // Act
            var roles = await _identityService.GetUserRolesAsync(userId);

            // Assert
            roles.Should().NotBeNullOrEmpty().And.Contain(role);
        }


        [Fact]
        public async Task GetAvailableUserRolesAsync_CorrectFlow_ReturnsMultipleRole()
        {
            // Arrange
            IList<string> roles = [Roles.Licensee, Roles.SystemOwner, Roles.Administrator, Roles.Coach];

            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(new AuthenticationRole(role));
            }

            // Act
            var result = await _identityService.GetAvailableUserRolesAsync();


            // Assert
            result.Should().NotBeNullOrEmpty().And.BeEquivalentTo(roles);
        }

        [Fact]
        public async Task GetUsersInRolesAsync_MultipleUsersWithDifferentRoles_ReturnsAllUsers()
        {
            // Arrange
            IList<string> roles = [Roles.Licensee, Roles.SystemOwner, Roles.Administrator, Roles.Coach];

            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(new AuthenticationRole(role));
            }

            var user1 = new AuthenticationUser { Id = "Test1", UserName = "Test1" };
            var user2 = new AuthenticationUser { Id = "Test2", UserName = "Test2" };
            var user3 = new AuthenticationUser { Id = "Test3", UserName = "Test3" };
            var user4 = new AuthenticationUser { Id = "Test4", UserName = "Test4" };

            var users = new List<AuthenticationUser> { user1, user2, user3, user4 };

            var counter = 0;
            foreach (var user in users)
            {
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, roles[counter]);
                counter++;
            }

            // Act
            var result = await _identityService.GetUsersInRolesAsync(Roles.Licensee, Roles.SystemOwner, Roles.Administrator, Roles.Coach);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(users);
        }

        [Theory]
        [InlineData(Roles.SystemOwner)]
        [InlineData(Roles.Licensee)]
        [InlineData(Roles.Administrator)]
        [InlineData(Roles.Coach)]
        public async Task GetUsersInRolesAsync_GetUserWithSpecificRole_ReturnsOneUserWithRole(string role)
        {
            // Arrange
            IList<string> roles = [Roles.Licensee, Roles.SystemOwner, Roles.Administrator, Roles.Coach];

            foreach (var r in roles)
            {
                await _roleManager.CreateAsync(new AuthenticationRole(r));
            }

            var user1 = new AuthenticationUser { Id = "Test1", UserName = "Test1" };
            var user2 = new AuthenticationUser { Id = "Test2", UserName = "Test2" };
            var user3 = new AuthenticationUser { Id = "Test3", UserName = "Test3" };
            var user4 = new AuthenticationUser { Id = "Test4", UserName = "Test4" };

            var users = new List<AuthenticationUser> { user1, user2, user3, user4 };

            var counter = 0;
            foreach (var user in users)
            {
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, roles[counter]);
                counter++;
            }

            // Act
            var result = await _identityService.GetUsersInRolesAsync(role);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async Task GetUsersInRolesAsync_UserWithMultipleRoles_ReturnsUserWithMultipleRoles()
        {
            // Arrange
            IList<string> roles = [Roles.Licensee, Roles.SystemOwner, Roles.Administrator, Roles.Coach];

            foreach (var r in roles)
            {
                await _roleManager.CreateAsync(new AuthenticationRole(r));
            }

            var user1 = new AuthenticationUser { Id = "Test1", UserName = "Test1" };
            var user2 = new AuthenticationUser { Id = "Test2", UserName = "Test2" };
            var user3 = new AuthenticationUser { Id = "Test3", UserName = "Test3" };
            var user4 = new AuthenticationUser { Id = "Test4", UserName = "Test4" };

            var users = new List<AuthenticationUser> { user1, user2, user3, user4 };

            var counter = 0;
            foreach (var user in users)
            {
                await _userManager.CreateAsync(user);

                if (counter == 0)
                {
                    foreach (var r in roles)
                    {
                        await _userManager.AddToRoleAsync(user, r);
                    }
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, roles[counter]);
                }

                counter++;
            }

            // Act
            var result = await _identityService.GetUsersInRolesAsync(Roles.Coach);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2);
            result.Should().ContainEquivalentOf(user1).And.ContainEquivalentOf(user4);
        }
    }
}

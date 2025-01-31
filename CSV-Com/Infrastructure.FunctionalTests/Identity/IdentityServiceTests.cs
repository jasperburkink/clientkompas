using Application.Common.Exceptions;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityServiceIntegrationTests()
        {
            var serviceCollection = new ServiceCollection()
            .AddLogging()
            .AddAuthorization()
            .AddDbContext<AuthenticationDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()))
            .AddIdentity<AuthenticationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

            serviceCollection.Services.AddScoped<IAuthenticationDbContext>(provider => provider.GetService<AuthenticationDbContext>());
            serviceCollection.Services.AddScoped<IHasher, Argon2Hasher>();
            serviceCollection.Services.AddScoped<ITokenService, TokenService>();

            // Build de ServiceProvider op de ServiceCollection, niet op de IdentityBuilder
            var serviceProvider = serviceCollection.Services.BuildServiceProvider();

            _userManager = serviceProvider.GetRequiredService<UserManager<AuthenticationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var signInManager = serviceProvider.GetRequiredService<SignInManager<AuthenticationUser>>();
            var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
            var hasher = serviceProvider.GetRequiredService<IHasher>();
            var refreshtokenService = serviceProvider.GetRequiredService<ITokenService>();

            _identityService = new IdentityService(_userManager, signInManager, null, authorizationService, hasher, refreshtokenService);
        }

        #region CreateUserAsync

        [Fact]
        public async Task CreateUserAsync_ValidInput_ShouldReturnSuccessResult()
        {
            // Arrange
            var userName = "test@example.com";
            var password = "Password123!";

            // Act
            var (result, userId) = await _identityService.CreateUserAsync(userName, password);

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
            var (result, _) = await _identityService.CreateUserAsync(userName, password);
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
            var (result, _) = await _identityService.CreateUserAsync(userName, "Password123!");
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
            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
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
            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
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
            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
            result.Succeeded.Should().BeTrue();

            // Assign role
            await _roleManager.CreateAsync(new IdentityRole(role));
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
            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
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

            var (result, userId) = await _identityService.CreateUserAsync(userName, password);

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

            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
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

            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
            await _roleManager.CreateAsync(new IdentityRole(role));
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

            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
            var token = "WrongToken";

            // Act
            var resultLogin = await _identityService.Login2FAAsync(userId, token);

            // Assert
            resultLogin.Should().NotBeNull();
            resultLogin.Succeeded.Should().BeFalse();
        }
    }
}

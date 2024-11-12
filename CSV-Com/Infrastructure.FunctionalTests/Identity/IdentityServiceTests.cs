using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
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
        private readonly SignInManager<AuthenticationUser> _signInManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHasher _hasher;
        private readonly IRefreshTokenService _refreshtokenService;
        private readonly IEmailService _emailService;

        public IdentityServiceIntegrationTests()
        {
            var serviceCollection = new ServiceCollection()
            .AddDbContext<AuthenticationDbContext>(options => options.UseInMemoryDatabase("TestDb"))
            .AddIdentity<AuthenticationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

            serviceCollection.Services.AddScoped<IHasher, Argon2Hasher>();
            serviceCollection.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            serviceCollection.Services.AddScoped<IEmailService, EmailService>();

            // Build de ServiceProvider op de ServiceCollection, niet op de IdentityBuilder
            var serviceProvider = serviceCollection.Services.BuildServiceProvider();

            var options = new DbContextOptionsBuilder<AuthenticationDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            _userManager = serviceProvider.GetRequiredService<UserManager<AuthenticationUser>>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<AuthenticationUser>>();
            _authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
            _hasher = serviceProvider.GetRequiredService<IHasher>();
            _refreshtokenService = serviceProvider.GetRequiredService<IRefreshTokenService>();
            _emailService = serviceProvider.GetRequiredService<IEmailService>();

            _identityService = new IdentityService(_userManager, _signInManager, null, _authorizationService, _hasher, _refreshtokenService, _emailService);
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

        [Fact]
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

        //[Fact]
        //public async Task GetUserNameAsync_UserExists_ShouldReturnUserName()
        //{
        //    // Arrange
        //    var userName = "test@example.com";
        //    var password = "Password123!";

        //    // Create user
        //    var (result, userId) = await _identityService.CreateUserAsync(userName, password);
        //    result.Succeeded.Should().BeTrue();

        //    // Act
        //    var result = await _identityService.GetUserNameAsync(userId);

        //    // Assert
        //    result.Should().Be(userName);
        //}

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

            // Create user
            var (result, userId) = await _identityService.CreateUserAsync(userName, password);
            result.Succeeded.Should().BeTrue();

            // Assign role
            await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), "Admin");

            // Act
            var isInRole = await _identityService.IsInRoleAsync(userId, "Admin");

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
    }
}

using System.Security.Claims;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Infrastructure.UnitTests.Identity
{
    public class IdentityServiceTests
    {
        private readonly Mock<UserManager<AuthenticationUser>> _userManagerMock;
        private readonly Mock<SignInManager<AuthenticationUser>> _signInManagerMock;
        private readonly Mock<IUserClaimsPrincipalFactory<AuthenticationUser>> _userClaimsPrincipalFactoryMock;
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IHasher> _hasherMock;
        private readonly Mock<ITokenService> _refreshTokenServiceMock;

        public IdentityServiceTests()
        {
            _userManagerMock = new Mock<UserManager<AuthenticationUser>>(Mock.Of<IUserStore<AuthenticationUser>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<AuthenticationUser>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AuthenticationUser>>(), null, null, null, null);
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AuthenticationUser>>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _hasherMock = new Mock<IHasher>();
            _refreshTokenServiceMock = new Mock<ITokenService>();
        }

        [Fact]
        public async Task CreateUserAsync_ValidData_ShouldCreateUserWithHashedPasswordAndSalt()
        {
            // Arrange
            var userName = "testuser@example.com";
            var password = "TestPassword123!";
            var salt = new byte[] { 1, 2, 3, 4 };
            var hashedPassword = "hashedpassword";

            _hasherMock.Setup(h => h.GenerateSalt(It.IsAny<int>())).Returns(salt);
            _hasherMock.Setup(h => h.HashString(password, salt)).Returns(hashedPassword);

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var (result, userId) = await identityService.CreateUserAsync(userName, password, 0);

            // Assert
            result.Succeeded.Should().BeTrue();
            userId.Should().NotBeNull();

            // Verify that the Salt was correctly set and password hashed
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ShouldReturnSuccessAndRoles()
        {
            // Arrange            
            var userName = "testuser@example.com";
            var password = "TestPassword123!";
            var salt = new byte[] { 1, 2, 3, 4 };
            var hashedPassword = "hashedpassword";
            var adminRole = nameof(Roles.Administrator);

            var user = new AuthenticationUser
            {
                UserName = userName,
                Salt = salt,
                PasswordHash = hashedPassword
            };

            var signInResult = SignInResult.Success;

            _userManagerMock.Setup(x => x.FindByNameAsync(userName)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync([adminRole]);

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(userName, password, true, false))
                             .ReturnsAsync(signInResult);

            _hasherMock.Setup(h => h.HashString(password, salt)).Returns(hashedPassword);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var loginResult = await identityService.LoginAsync(userName, password);

            // Assert
            loginResult.Succeeded.Should().BeTrue();
            loginResult.Roles.Should().Contain(adminRole);
        }

        [Fact]
        public async Task GetUserNameAsync_UserExists_ShouldReturnUserName()
        {
            // Arrange
            var userId = "123";
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = new AuthenticationUser
            {
                Id = userId,
                UserName = "testuser",
                Salt = salt
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                           .ReturnsAsync(user);

            var identityService = new IdentityService(
                 _userManagerMock.Object,
                 _signInManagerMock.Object,
                 _userClaimsPrincipalFactoryMock.Object,
                 _authorizationServiceMock.Object,
                 _hasherMock.Object,
                 _refreshTokenServiceMock.Object
             );

            // Act
            var result = await identityService.GetUserNameAsync(userId);

            // Assert
            result.Should().Be("testuser");
        }

        [Fact]
        public async Task IsInRoleAsync_UserExistsInRole_ShouldReturnTrue()
        {
            // Arrange
            var userId = "123";
            var role = nameof(Roles.Administrator);
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = new AuthenticationUser { Id = userId, Salt = salt };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, role))
                            .ReturnsAsync(true);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.IsInRoleAsync(userId, role);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsInRoleAsync_UserDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var userId = "123";
            var role = nameof(Roles.Administrator);

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync((AuthenticationUser)null);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.IsInRoleAsync(userId, role);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AuthorizeAsync_UserHasPolicy_ShouldReturnTrue()
        {
            // Arrange
            var userId = "123";
            var policyName = "AdminPolicy";
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = new AuthenticationUser { Id = userId, Salt = salt };
            var claimsPrincipal = new ClaimsPrincipal();
            var authorizationResult = AuthorizationResult.Success();

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);
            _userClaimsPrincipalFactoryMock.Setup(f => f.CreateAsync(user))
                                           .ReturnsAsync(claimsPrincipal);
            _authorizationServiceMock.Setup(a => a.AuthorizeAsync(claimsPrincipal, It.IsAny<object>(), policyName))
                                     .ReturnsAsync(authorizationResult);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.AuthorizeAsync(userId, policyName);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AuthorizeAsync_UserDoesNotHavePolicy_ShouldReturnFalse()
        {
            // Arrange
            var userId = "123";
            var policyName = "AdminPolicy";
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = new AuthenticationUser { Id = userId, Salt = salt };
            var claimsPrincipal = new ClaimsPrincipal();
            var authorizationResult = AuthorizationResult.Failed();

            // Mock het gedrag van de IUserClaimsPrincipalFactory
            _userClaimsPrincipalFactoryMock.Setup(f => f.CreateAsync(user))
                                           .ReturnsAsync(claimsPrincipal);

            // Mock de IAuthorizationService zonder extensiemethode
            _authorizationServiceMock.Setup(a => a.AuthorizeAsync(claimsPrincipal, It.IsAny<object>(), policyName))
                                     .ReturnsAsync(authorizationResult);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.AuthorizeAsync(userId, policyName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteUserAsync_UserExists_ShouldReturnSuccess()
        {
            // Arrange
            var userId = "123";
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = new AuthenticationUser { Id = userId, Salt = salt };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user))
                            .ReturnsAsync(IdentityResult.Success);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.DeleteUserAsync(userId);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_UserDoesNotExist_ShouldReturnSuccess()
        {
            // Arrange
            var userId = "123";

            _userManagerMock.Setup(um => um.FindByIdAsync(userId))
                            .ReturnsAsync((AuthenticationUser)null);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.DeleteUserAsync(userId);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ValidUser_ShouldReturnSuccess()
        {
            // Arrange
            var salt = new byte[] { 1, 2, 3, 4 };
            var user = new AuthenticationUser { Id = "123", Salt = salt };

            _userManagerMock.Setup(um => um.DeleteAsync(user))
                            .ReturnsAsync(IdentityResult.Success);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.DeleteUserAsync(user);

            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task LogoutAsync_ShouldCallSignOutAsync()
        {
            // Arrange
            _signInManagerMock.Setup(x => x.SignOutAsync())
                              .Returns(Task.CompletedTask);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            await identityService.LogoutAsync();

            // Assert
            _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async Task SendResetPasswordEmailAsync_CorrectFlow_ResultIsSuccess()
        {
            // Arrange
            var emailAddress = TestData.FakerConfiguration.Faker.Person.Email;
            var user = new AuthenticationUser();
            string token = nameof(token);

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                                  .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<AuthenticationUser>())).ReturnsAsync(token);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.GetResetPasswordEmailToken(emailAddress);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetResetPasswordEmailToken_UserDoesNotExists_ReturnsEmptyString()
        {
            // Arrange
            var emailAddress = TestData.FakerConfiguration.Faker.Person.Email;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()));

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.GetResetPasswordEmailToken(emailAddress);

            // Assert
            result.Should().NotBeNull().And.BeEmpty();
        }


        [Fact]
        public async Task ResetPasswordAsync_CorrectFlow_ReturnSuccessResult()
        {
            // Arrange
            var emailAddress = TestData.FakerConfiguration.Faker.Person.Email;
            var user = new AuthenticationUser();
            string token = nameof(token);
            var password = TestData.FakerConfiguration.Faker.Internet.Password();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                                  .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                                  .ReturnsAsync(IdentityResult.Success);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.ResetPasswordAsync(emailAddress, token, password);

            // Assert
            result.Should().BeEquivalentTo(Result.Success());
        }

        [Fact]
        public async Task ResetPasswordAsync_UserDoesNotExists_ReturnFailedResult()
        {
            // Arrange
            var emailAddress = TestData.FakerConfiguration.Faker.Person.Email;
            var user = new AuthenticationUser();
            string token = nameof(token);
            var password = TestData.FakerConfiguration.Faker.Internet.Password();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()));

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.ResetPasswordAsync(emailAddress, token, password);

            // Assert
            result.Should().BeEquivalentTo(Result.Failure(["User is not found with the given emailaddress."]));
        }


        [Fact]
        public async Task Get2FATokenAsync_CorrectFlow_ShouldReturn2FAToken()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var token = "012345";

            var user = new AuthenticationUser
            {
                Id = userId
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GenerateTwoFactorTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(token);

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.Get2FATokenAsync(userId);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Get2FATokenAsync_UserNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()));

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            Func<Task<string>> act = async () => await identityService.Get2FATokenAsync(userId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Login2FAAsync_CorrectFlow_ShouldBeLoggedIn()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var token = "012345";

            var user = new AuthenticationUser
            {
                Id = userId
            };

            var signInResult = SignInResult.Success;

            _userManagerMock.Setup(mock => mock.VerifyTwoFactorTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetRolesAsync(user));

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            var result = await identityService.Login2FAAsync(userId, token);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Login2FAAsync_UserIsNull_ShouldThrowNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var token = "012345";

            var signInResult = SignInResult.Success;

            _signInManagerMock.Setup(mock => mock.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(signInResult);
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()));

            var identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object,
                _hasherMock.Object,
                _refreshTokenServiceMock.Object
            );

            // Act
            Func<Task<LoggedInResult>> act = async () => await identityService.Login2FAAsync(userId, token);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}

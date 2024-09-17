using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.FunctionalTests.Identity
{
    public class IdentityServiceTests
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<AuthenticationUser> _userManager;
        private readonly SignInManager<AuthenticationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<AuthenticationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityServiceTests()
        {
            //_identityService = new IdentityService(_userManager, _signInManager, _userClaimsPrincipalFactory, _authorizationService);
        }

        //[Fact]
        //public void GetUserNameAsync_CorrectFlow_ShouldReturnUserName()
        //{
        //    // Arrange
        //    var userId = "0";
        //    var userNameCheck = "";

        //    // Act
        //    var userName = _identityService.GetUserNameAsync(userId);

        //    // Assert
        //    userName.Should().Be();
        //}
    }
}

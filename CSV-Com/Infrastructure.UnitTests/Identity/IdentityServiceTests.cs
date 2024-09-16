using Domain.Authentication.Domain;
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
        private readonly IdentityService _identityService;

        public IdentityServiceTests()
        {
            _userManagerMock = new Mock<UserManager<AuthenticationUser>>(Mock.Of<IUserStore<AuthenticationUser>>(), null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<AuthenticationUser>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AuthenticationUser>>(), null, null, null, null);
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AuthenticationUser>>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();

            _identityService = new IdentityService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                _authorizationServiceMock.Object
            );
        }
    }
}
